import { MESSAGE_API } from '../apiConsts.ts';

export const sendMessage = async (
  token: string, 
  receiverId: number, 
  content: string 
): Promise<{ success: boolean; error?: string }> => {
  try {
    // Message payload
    const messageData = {
      receiverId,
      content,
    };

    const response = await fetch(MESSAGE_API.SEND_MESSAGE, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify(messageData), 
    });

    if (response.ok) {
      return { success: true };
    } else {
      const errorData = await response.json();
      return { success: false, error: errorData.title || "Failed to send message." };
    }
  } catch (error) {
    console.error("Error sending message:", error);
    return { success: false, error: "An unexpected error occurred." };
  }
};
