import React, { useEffect, useRef, useState } from "react";
import "./messages.css";
import audio from "../../assets/images/audio.png";
import video from "../../assets/images/video.png";
import arrow from "../../assets/images/arrow.png";
import { MessageTab } from "../messageTab/MessageTab.tsx";
import {
  getConversation,
  Message,
} from "../../api/Messages/getConversationRequest.ts";
import { sendMessage } from "../../api/Messages/postMessageRequest.ts";
import API_BASE_URL from "../../api/apiConsts.ts";
import { HubConnectionBuilder, HubConnection } from "@microsoft/signalr";

type UserType = {
  userId: number;
  userName: string;
  profilePicturePath: string;
};

type MessagesProps = {
  currentUserId: number;
  otherUserId: number;
  token: string;
  users: UserType[];
};

export const Messages: React.FC<MessagesProps> = ({
  currentUserId,
  otherUserId,
  token,
  users,
}) => {
  const [showMessagesTab, setShowMessagesTab] = useState(false);
  const [messages, setMessages] = useState<Message[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [newMessage, setNewMessage] = useState<string>("");

  // Call-related states
  const [callConnection, setCallConnection] = useState<HubConnection | null>(
    null
  );
  const [peerConnection, setPeerConnection] =
    useState<RTCPeerConnection | null>(null);
  const [localStream, setLocalStream] = useState<MediaStream | null>(null);
  const [remoteStream, setRemoteStream] = useState<MediaStream | null>(null);

  // Incoming call states
  const [incomingCall, setIncomingCall] = useState(false);
  const [incomingCallUser, setIncomingCallUser] = useState<string>("");
  const [incomingOffer, setIncomingOffer] =
    useState<RTCSessionDescriptionInit | null>(null);

  // Timeout to automatically end call if not answered
  let callTimeout: NodeJS.Timeout | null = null;

  const remoteAudioRef = useRef<HTMLAudioElement>(null);
  const localAudioRef = useRef<HTMLAudioElement>(null);
  const messagesEndRef = useRef<HTMLDivElement>(null);

  const otherUser = users.find((user) => user.userId === otherUserId);

  // Fetch messages
  useEffect(() => {
    const fetchMessages = async () => {
      setLoading(true);
      setError(null);

      try {
        const response = await getConversation(token, otherUserId);
        if (response.success && response.data) {
          const sortedMessages = response.data.sort(
            (a: Message, b: Message) =>
              new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime()
          );
          sortedMessages.reverse();
          setMessages(sortedMessages);
        } else {
          setError(response.error || "Failed to fetch messages.");
        }
      } catch (err) {
        console.error("Error fetching messages:", err);
        setError("An unexpected error occurred.");
      } finally {
        setLoading(false);
      }
    };

    fetchMessages();
  }, [token, otherUserId]);

  // Scroll to bottom of message list
  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [messages]);

  const handleSendMessage = async () => {
    if (newMessage.trim() === "") return;
    const result = await sendMessage(token, otherUserId, newMessage);

    if (result.success) {
      const newMessageObject: Message = {
        messageId: Date.now(),
        senderId: currentUserId,
        receiverId: otherUserId,
        content: newMessage,
        isDeletedBySender: false,
        isDeletedByReceiver: false,
        createdAt: new Date().toISOString(),
      };
      setMessages((prevMessages) => [newMessageObject, ...prevMessages]);
      setNewMessage("");
    } else {
      setError(result.error || "Failed to send message.");
    }
  };

  const handleKeyDown = (event: React.KeyboardEvent) => {
    if (event.key === "Enter") {
      event.preventDefault();
      handleSendMessage();
    }
  };

  // Initialize SignalR connection for calls
  useEffect(() => {
    const initCallConnection = async () => {
      if (!token) return;

      const conn = new HubConnectionBuilder()
        .withUrl(
          "https://vibez-web-service-g8gzbmfvdnc2hahw.northeurope-01.azurewebsites.net/callHub",
          {
            accessTokenFactory: () => token,
          }
        )
        .withAutomaticReconnect()
        .build();

      conn.on("ReceiveOffer", async (offer: RTCSessionDescriptionInit) => {
        console.log("Received offer:", offer);
        const caller = otherUser ? otherUser.userName : "Unknown User";
        setIncomingCallUser(caller);
        setIncomingOffer(offer);
        setIncomingCall(true);
      });

      conn.on("ReceiveAnswer", async (answer: RTCSessionDescriptionInit) => {
        console.log("Received answer:", answer);
        if (peerConnection) {
          await peerConnection.setRemoteDescription(
            new RTCSessionDescription(answer)
          );
        }
        // If we had a timeout running (waiting for answer), clear it:
        if (callTimeout) {
          clearTimeout(callTimeout);
          callTimeout = null;
        }
      });

      conn.on("ReceiveIceCandidate", (candidate: RTCIceCandidateInit) => {
        console.log("Received ICE candidate:", candidate);
        if (peerConnection && candidate && conn.state === "Connected") {
          peerConnection.addIceCandidate(new RTCIceCandidate(candidate));
        }
      });

      conn.on("CallEnded", () => {
        console.log("Call ended by other user.");
        endCall();
      });

      try {
        await conn.start();
        console.log("SignalR connection established. State:", conn.state);
        setCallConnection(conn);
      } catch (err) {
        console.error("Failed to start SignalR connection:", err);
      }
    };

    initCallConnection();

    return () => {
      callConnection?.stop();
    };
  }, []);

  const createPeerConnection = async () => {
    const pc = new RTCPeerConnection();

    pc.ontrack = (event) => {
      const [stream] = event.streams;
      setRemoteStream(stream);
      if (remoteAudioRef.current) {
        remoteAudioRef.current.srcObject = stream;
      }
    };

    pc.onicecandidate = (event) => {
      if (
        event.candidate &&
        callConnection &&
        callConnection.state === "Connected"
      ) {
        callConnection
          .invoke("SendIceCandidate", otherUserId.toString(), event.candidate)
          .catch((err) => console.error("Failed to send ICE candidate:", err));
      }
    };

    // Get local media if not already
    if (!localStream) {
      const localMedia = await navigator.mediaDevices.getUserMedia({
        audio: true,
        video: false,
      });
      setLocalStream(localMedia);
      if (localAudioRef.current) {
        localAudioRef.current.srcObject = localMedia;
      }
      localMedia.getTracks().forEach((track) => {
        pc.addTrack(track, localMedia);
      });
    } else {
      localStream.getTracks().forEach((track) => {
        pc.addTrack(track, localStream);
      });
    }

    setPeerConnection(pc);
    return pc;
  };

  const startCall = async () => {
    if (!callConnection || callConnection.state !== "Connected") {
      console.warn("Trying to start call while not connected.");
      return;
    }
    const pc = await createPeerConnection();
    const offer = await pc.createOffer();
    await pc.setLocalDescription(offer);

    try {
      await callConnection.invoke("SendOffer", otherUserId.toString(), offer);
      console.log("Offer sent successfully");

      // Start a 30-second timer if callee doesn't answer
      callTimeout = setTimeout(() => {
        console.log("No answer received within 30 seconds, ending call.");
        endCall();
      }, 30000);
    } catch (err) {
      console.error("Failed to send offer:", err);
    }
  };

  const acceptCall = async () => {
    setIncomingCall(false);
    if (
      !callConnection ||
      callConnection.state !== "Connected" ||
      !incomingOffer
    ) {
      console.warn("Trying to accept call while not connected or no offer.");
      return;
    }

    const pc = await createPeerConnection();
    await pc.setRemoteDescription(new RTCSessionDescription(incomingOffer));
    const answer = await pc.createAnswer();
    await pc.setLocalDescription(answer);

    try {
      await callConnection.invoke(
        "SendAnswer",
        currentUserId.toString(),
        answer
      );
      console.log("Answer sent successfully");
    } catch (err) {
      console.error("Failed to send answer:", err);
    }

    setIncomingOffer(null);
    // If there was a timeout on the caller side, the caller will clear it once they receive the answer.
  };

  const rejectCall = () => {
    setIncomingCall(false);
    setIncomingOffer(null);
    // If you implement a "CallRejected" signal on the backend, you can send it here.
  };

  const endCall = () => {
    if (peerConnection) {
      peerConnection.close();
      setPeerConnection(null);
    }

    if (localStream) {
      localStream.getTracks().forEach((track) => track.stop());
      setLocalStream(null);
    }

    if (remoteStream) {
      remoteStream.getTracks().forEach((track) => track.stop());
      setRemoteStream(null);
    }

    if (localAudioRef.current) localAudioRef.current.srcObject = null;
    if (remoteAudioRef.current) remoteAudioRef.current.srcObject = null;

    if (callConnection && callConnection.state === "Connected") {
      callConnection
        .invoke("EndCall", otherUserId.toString())
        .catch((err) => console.error("Failed to end call:", err));
    }

    // Clear any running call timeout
    if (callTimeout) {
      clearTimeout(callTimeout);
      callTimeout = null;
    }
  };

  const getImageUrl = (imageUrl: string | null) =>
    imageUrl ? `${API_BASE_URL}${imageUrl}` : undefined;

  if (showMessagesTab) {
    return <MessageTab currentUserId={currentUserId} token={token} />;
  }

  return (
    <div className="messages-container">
      <div className="messages-header">
        <img
          src={arrow}
          alt="arrow"
          className="back-arrow"
          onClick={() => setShowMessagesTab(true)}
        />
        {otherUser && (
          <div className="user-info">
            <img
              src={getImageUrl(otherUser.profilePicturePath)}
              alt={otherUser.userName}
              className="user-profile-pic"
            />
            <span className="user-name">{otherUser.userName}</span>
          </div>
        )}
      </div>

      <div className="messages-list">
      <div ref={messagesEndRef} />
        {loading ? (
          <p>Loading messages...</p>
        ) : error ? (
          <p className="error-message">{error}</p>
        ) : messages.length === 0 ? (
          <p>No messages to display.</p>
        ) : (
          messages.map((message) => (
            <div
              key={message.messageId}
              className={`message ${
                message.senderId === currentUserId ? "sent" : "received"
              }`}
            >
              <p className="message-content">{message.content}</p>
              <span className="message-timestamp">
                {new Date(message.createdAt).toLocaleTimeString()}
              </span>
            </div>
          ))
        )}
      </div>

      <div style={{ display: "none" }}>
        <audio ref={localAudioRef} autoPlay muted />
        <audio ref={remoteAudioRef} autoPlay />
      </div>

      <div className="message-input-container">
        <input
          type="text"
          value={newMessage}
          onChange={(e) => setNewMessage(e.target.value)}
          onKeyDown={handleKeyDown}
          placeholder="Send a Message"
        />
        <img
          src={audio}
          alt="Audio Call"
          className="audio-button"
          onClick={startCall}
        />
        <img
          src={video}
          alt="End Call"
          className="video-button"
          onClick={endCall}
        />
      </div>

      {incomingCall && (
        <div className="incoming-call-modal">
          <div className="incoming-call-content">
            <p>{incomingCallUser} is calling you...</p>
            <button onClick={acceptCall}>Accept</button>
            <button onClick={rejectCall}>Reject</button>
          </div>
        </div>
      )}
    </div>
  );
};
