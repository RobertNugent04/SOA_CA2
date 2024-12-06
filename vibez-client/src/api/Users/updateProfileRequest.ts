import { USER_API } from '../apiConsts.ts';

interface UpdateProfilePayload {
  fullName: string;
  bio: string;
  profilePicture: File | null;
}

export const updateProfileRequest = async (
  payload: UpdateProfilePayload,
  token: string
) => {
  const formData = new FormData();
  formData.append('FullName', payload.fullName);
  formData.append('Bio', payload.bio);
  if (payload.profilePicture) {
    formData.append('ProfilePicture', payload.profilePicture);
  }

  try {
    const response = await fetch(USER_API.UPDATE_USER, {
      method: 'PUT',
      headers: {
        Authorization: `Bearer ${token}`,
      },
      body: formData,
    });

    if (response.ok) {
      // Handle response with content
      if (response.status !== 204) {
        const data = await response.json(); // Parse response as JSON
        console.log('Profile update successful:', data);
        return { success: true, data };
      } else {
        console.log('Profile updated successfully with no content.');
        return { success: true, data: null }; // Handle 204 No Content
      }
    } else {
      // Parse error details
      const errorData = await response.json();
      console.error('Profile update error:', errorData);
      return { success: false, message: 'Profile update failed.', error: errorData };
    }
  } catch (error) {
    console.error('Error during profile update request:', error);
    return { success: false, message: 'An error occurred while updating the profile.', error };
  }
};
