import { POST_API } from "../apiConsts.ts";

type Post = {
  postId: number;
  userId: number;
  content: string;
  imageUrl: string | null;
  userName: string;
  profilePicturePath: string | null;
  createdAt: string;
};

export const getPostRequest = async (
  postId: number,
  token: string
): Promise<{
  success: boolean;
  data?: Post;
  error?: string;
}> => {
  try {
    const response = await fetch(POST_API.GET_POST(postId), {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });

    console.log("Response:", response);

    if (response.ok) {
      const data: Post = await response.json();
      console.log("Post fetched successfully:", data);
      return { success: true, data };
    } else {
      const errorData = await response.json();
      return { success: false, error: errorData.title || "An error occurred." };
    }
  } catch (error) {
    console.error("Request failed:", error);
    return { success: false, error: "An unexpected error occurred." };
  }
};
