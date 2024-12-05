const endpoint = 'http://localhost:5225/api/users/register';

interface RegisterPayload {
    fullName: string;
    userName: string;
    email: string;
    password: string;
  }

export const registerRequest = async (payload: RegisterPayload)=> {

  try {
    const response = await fetch(endpoint, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(payload),
    });

    if (response.ok) {
      const data = await response.json();
      console.log('Register successful:', data);
      return data; 
    }
    else {
      const errorData = await response.json();
      console.error('Login error:', errorData);
      return 'Login failed. Please check your credentials and token.';
    }

  } 
  
  catch (error) {
    console.error('Error during login request:', error);
    return 'An error occurred while submitting the login request.';
  }
};