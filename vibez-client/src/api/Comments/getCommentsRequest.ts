import { COMMENT_API } from '../apiConsts.ts';

export const getCommentsRequest = async (postId: number, token: string) => {
  // Create FormData and append the postId
  const formData = new FormData();
  formData.append("postId", postId.toString());

  try {
    const response = await fetch(`${COMMENT_API.GET_COMMENTS}`, {
      method: "GET",
      headers: {
        Authorization: `Bearer ${token}`,
      },
      body: formData, 
    });

    if (response.ok) {
      const data = await response.json(); 
      return { success: true, data };
    } else {
      const errorData = await response.json(); 
      console.error("Failed to fetch comments:", errorData);
      return { success: false, error: errorData.message || "Failed to fetch comments." };
    }
  } catch (error) {
    console.error("Error fetching comments:", error);
    return { success: false, error: "An error occurred while fetching comments." };
  }
};
