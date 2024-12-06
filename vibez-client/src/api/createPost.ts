import { POST_API } from "./apiConsts.ts";

interface CreatePostPayload {
  content: string;
  imageUrl: string;
}

export const createPostRequest = async (payload: CreatePostPayload, token: string) => {
  try {
    const response = await fetch(POST_API.CREATE_POSTS, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify(payload),
    });

    if (response.ok) {
      const data = await response.json();
      console.log("Post created successfully:", data);
      return { success: true, data };
    } else {
      const errorData = await response.json();
      console.error("Post creation error:", errorData);
      return { success: false, message: errorData.message || "Failed to create post." };
    }
  } catch (error) {
    console.error("Error during post creation request:", error);
    return { success: false, message: "An unexpected error occurred." };
  }
};
