import { FRIEND_API } from '../apiConsts.ts';

interface SendFriendRequestPayload {
  friendId: number;
}

export const sendFriendRequest = async (
  payload: SendFriendRequestPayload,
  token: string
) => {
  try {
    const response = await fetch(`${FRIEND_API.SEND_REQUEST}`, {
      method: 'POST',
      headers: {
        Authorization: `Bearer ${token}`,
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(payload),
    });

    if (!response.ok) {
      const errorData = await response.json();
      console.error('Error sending friend request:', errorData);
      return { success: false, error: errorData };
    }

    const data = await response.json();
    return { success: true, data };
  } catch (error) {
    console.error('Error sending friend request:', error);
    return { success: false, error };
  }
};
