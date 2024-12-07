import { MESSAGE_API } from '../apiConsts.ts';

export type Message = {
    messageId: number;
    senderId: number;
    receiverId: number;
    content: string;
    isDeletedBySender: boolean;
    isDeletedByReceiver: boolean;
    createdAt: string;
  };
  
  export const getConversation = async (
    token: string,
    friendId: number
  ): Promise<{
    success: boolean;
    data?: Message[];
    error?: string;
  }> => {
    try {
      const response = await fetch(MESSAGE_API.GET_CONVERSATION(friendId), {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
      });
  
      if (response.ok) {
        const data: Message[] = await response.json();
        return { success: true, data };
      } else {
        const errorData = await response.json();
        return { success: false, error: errorData.title || "Failed to fetch conversation." };
      }
    } catch (error) {
      console.error("Error fetching conversation:", error);
      return { success: false, error: "An unexpected error occurred." };
    }
  };
  