import { USER_API } from './apiConsts.ts';

// Function to fetch a user's profile by userId
type UserProfileResponse = {
    userId: number;
    fullName: string;
    userName: string;
    email: string;
    bio: string | null;
    profilePicturePath: string | null;
    createdAt: string;
    isActive: boolean;
  };


export const getUserProfileRequest = async ( token: string): Promise<{
  success: boolean;
  data?: UserProfileResponse;
  error?: string;
}> => {
  try {
    const response = await fetch(`${USER_API.GET_CURRENT_USER}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
    });

    console.log('Response:', response);

    if (response.ok) {
      const data: UserProfileResponse = await response.json();
      console.log('User profile fetched successfully:', data);
      return { success: true, data };
    } else {
      let errorData;
      try {
        errorData = await response.json();
      } catch {
        errorData = await response.text();
      }
      console.error('Error fetching user profile:', errorData);
      return { success: false, error: errorData };
    }
  } catch (error) {
    console.error('Request failed:', error);
    return { success: false, error: 'An unexpected error occurred while fetching the user profile.' };
  }
};