const endpoint = 'https://localhost:7198/api/users/search';

export const searchUserRequest = async (userName: string) => {
  try {
    const response = await fetch(endpoint, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ userName }),
    });

    if (response.ok) {
      const data = await response.json();
      console.log('Search response:', data);
      return data; 
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
