import { COMMENT_API } from '../apiConsts.ts';

interface CreateCommentPayload {
  content: string;
  postId: number;
}

export const createCommentRequest = async (
  payload: CreateCommentPayload,
  token: string
) => {
  try {
    const response = await fetch(`${COMMENT_API.CREATE_COMMENT}`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify(payload),
    });

    if (response.ok) {
      const data = await response.json();
      console.log("Comment created successfully:", data);
      return { success: true, data };
    } else {
      const errorData = await response.json();
      console.error("Failed to create comment:", errorData);
      return { success: false, message: "Failed to create comment.", error: errorData };
    }
  } catch (error) {
    console.error("Error during comment creation request:", error);
    return { success: false, message: "An error occurred while creating the comment.", error };
  }
};
