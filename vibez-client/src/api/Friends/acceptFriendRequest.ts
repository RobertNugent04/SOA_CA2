import { FRIEND_API } from '../apiConsts.ts'; 

// Function to accept a friendship request
export const acceptFriendshipRequest = async (
  token: string,
  friendId: number
): Promise<{ success: boolean; error?: string }> => {
  try {
    const response = await fetch(`${FRIEND_API.ACCEPT_REQUEST}/${friendId}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
    });

    if (response.ok) {
      return { success: true };
    } else {
      let errorData;
      try {
        errorData = await response.json();
      } catch {
        errorData = await response.text();
      }
      console.error('Error accepting friendship request:', errorData);
      return { success: false, error: errorData };
    }
  } catch (error) {
    console.error('Request failed:', error);
    return { success: false, error: 'An unexpected error occurred.' };
  }
};
