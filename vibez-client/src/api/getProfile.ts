import { USER_API } from "./apiConsts.ts";

type UserProfile = {
  userId: number;
  fullName: string;
  userName: string;
  email: string;
  bio: string | null;
  profilePicturePath: string | null;
  createdAt: string;
  isActive: boolean;
};

type Post = {
  postId: number;
  userId: number;
  content: string;
  imageUrl: string | null;
  createdAt: string;
};

type Friend = {
  friendshipId: number;
  userId: number;
  friendId: number;
  status: string;
};

type UserProfileResponse = {
  user: UserProfile;
  posts: Post[];
  friends: Friend[];
};

export const getProfileRequest = async (
  token: string,
  userId: number
): Promise<{
  success: boolean;
  data?: UserProfileResponse;
  error?: string;
}> => {
  try {
    const response = await fetch(USER_API.GET_USER(userId), {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
      });

    console.log("Response:", response);

    if (response.ok) {
      console.log("before")
      const data: UserProfileResponse = await response.json();
      console.log("Profile fetched successfully:", data);
      console.log("User: ", data.user)
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
