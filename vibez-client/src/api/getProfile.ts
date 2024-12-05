import { AUTH_API } from './apiConsts.ts';

// Function to fetch the user profile
type ProfileResponse = {
  userId: string;
  fullName: string;
  userName: string;
  email: string;
  bio: string;
  profilePicturePath: string;
  createdAt: string;
  isActive: boolean;
};

export const getProfileRequest = async (token: string): Promise<{
  success: boolean;
  data?: ProfileResponse;
  error?: string;
}> => {
  try {
    const response = await fetch(AUTH_API.GET_USER, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`, 
      },
    });

    if (response.ok) {
      const data: ProfileResponse = await response.json();
      console.log('Profile fetched successfully:', data);
      return { success: true, data };
    } else {
      let errorData;
      try {
        errorData = await response.json();
      } catch {
        errorData = await response.text(); // Handle non-JSON error responses
      }
      console.error('Error fetching profile:', errorData);
      return { success: false, error: errorData };
    }
  } catch (error) {
    console.error('Request failed:', error);
    return { success: false, error: 'An unexpected error occurred while fetching the profile.' };
  }
};
