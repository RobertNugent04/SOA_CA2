import { POST_API } from '../apiConsts.ts';

interface CreatePostPayload {
  content: string;
  imageUrl: File | null;
}

export const createPost = async (
  payload: CreatePostPayload,
  token: string
) => {
  const formData = new FormData();
  formData.append('Content', payload.content);
  if (payload.imageUrl) {
    formData.append('ImageUrl', payload.imageUrl);
  }

  try {
    const response = await fetch(POST_API.CREATE_POSTS, {
      method: 'POST',
      headers: {
        Authorization: `Bearer ${token}`,
      },
      body: formData,
    });

    if (response.ok) {
      // Handle response with content
      if (response.status !== 204) {
        const data = await response.json(); // Parse response as JSON
        console.log('Post created successfully:', data);
        return { success: true, data };
      } else {
        console.log('Post created successfully with no content.');
        return { success: true, data: null }; // Handle 204 No Content
      }
    } else {
      // Parse error details
      const errorData = await response.json();
      console.error('Post creation error:', errorData);
      return { success: false, message: 'Post creation failed.', error: errorData };
    }
  } catch (error) {
    console.error('Error during post creation request:', error);
    return { success: false, message: 'An error occurred while creating the post.', error };
  }
};
