import { LIKE_API } from '../apiConsts.ts';

export const likePostRequest = async (
  payload: { postId: number },
  token: string
): Promise<{ success: boolean; error?: string }> => {
  try {
    const response = await fetch(LIKE_API.SEND_LIKE, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify(payload),
    });

    if(response.ok) {
        console.log('Post liked successfully');
        }

    if (!response.ok) {
      const errorData = await response.json();
      return { success: false, error: errorData.message || "Failed to like post." };
    }

    return { success: true };
  } catch (error) {
    console.error("Error liking post:", error);
    return { success: false, error: "An error occurred while liking the post." };
  }
};
