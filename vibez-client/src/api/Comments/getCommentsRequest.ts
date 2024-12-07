import { COMMENT_API } from '../apiConsts.ts';

export const getCommentsRequest = async (postId: number, token: string) => {
  try {
    const response = await fetch(COMMENT_API.GET_COMMENTS(postId), {
      method: 'GET',
      headers: {
        Authorization: `Bearer ${token}`,
        'Content-Type': 'application/json',
      },
    });

    if (!response.ok) {
      const errorData = await response.json();
      console.error('Error fetching comments:', errorData);
      return { success: false, error: errorData };
    }

    const data = await response.json();
    return { success: true, data };
  } catch (error) {
    console.error('Error fetching comments:', error);
    return { success: false, error };
  }
};
