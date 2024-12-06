import { AUTH_API } from './apiConsts.ts';

interface LoginPayload {
  userNameOrEmail: string;
  password: string;
}

export const loginRequest = async (payload: LoginPayload) => {
  try {
    const response = await fetch(AUTH_API.LOGIN, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(payload),
    });

    if (response.ok) {
      const data = await response.json();
      console.log('Login successful:', data);
      return { success: true, data };
    } else {
      const errorData = await response.json();
      console.error('Login error:', errorData);
      return { success: false, message: errorData.message || 'Login failed.' };
    }
  } catch (error) {
    console.error('Error during login request:', error);
    return { success: false, message: 'An unexpected error occurred.' };
  }
};
