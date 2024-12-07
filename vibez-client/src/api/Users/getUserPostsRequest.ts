import { POST_API } from "./apiConsts.ts";

type Post = {
  postId: number;
  userId: number;
  content: string;
  imageUrl: string | null;
  createdAt: string;
};

export const getUserPostsRequest = async (
  token: string,
  userId: number
): Promise<{
  success: boolean;
  data?: Post[];
  error?: string;
}> => {
  try {
    const response = await fetch(POST_API.GET_USER_POSTS(userId), {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });

    if (response.ok) {
      const data: Post[] = await response.json();
      console.log("Posts fetched successfully:", data);
      return { success: true, data };
    } else {
      const errorData = await response.json();
      console.error("Error response from API:", errorData);
      return { success: false, error: errorData.title || "An error occurred." };
    }
  } catch (error) {
    console.error("Request failed:", error);
    return { success: false, error: "An unexpected error occurred." };
  }
};
