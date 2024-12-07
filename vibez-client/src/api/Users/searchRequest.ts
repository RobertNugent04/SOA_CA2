import { USER_API } from '../apiConsts.ts';

export const searchRequest = async (query: string) => {
  try {
    // Use 'query' instead of 'userName' as the parameter name
    const url = `${USER_API.SEARCH}?query=${encodeURIComponent(query)}`;

    const response = await fetch(url, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
    });

    if (response.ok) {
      const data = await response.json();
      console.log('Search response:', data);
      return data; // Return the search results
    } else {
      const errorData = await response.json();
      console.error('Search error:', errorData);
      throw new Error('Search failed.');
    }
  } catch (error) {
    console.error('Error during search request:', error);
    throw error;
  }
};
