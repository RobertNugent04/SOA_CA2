const endpoint = 'https://localhost:7198/api/users/request-password-reset';

interface resetRequestPayload {
email: string;
}

export const resetPasswordRequest = async (payload: resetRequestPayload) => {
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
      console.log('request successful:', data);
      return { success: true, data };
    } else {
      const errorData = await response.json();
      console.error('request error:', errorData);
      return { success: false, message: errorData.message || 'request failed.' };
    }
  } catch (error) {
    console.error('Error during reset request:', error);
    return { success: false, message: 'An unexpected error occurred.' };
  }
};

interface changePasswordPayload {
    email: string;
    otp: string;
    newPassword: string;
    }
    
    export const changePassword = async (payload: changePasswordPayload) => {
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
          console.log('request successful:', data);
          return { success: true, data };
        } else {
          const errorData = await response.json();
          console.error('request error:', errorData);
          return { success: false, message: errorData.message || 'request failed.' };
        }
      } catch (error) {
        console.error('Error during reset request:', error);
        return { success: false, message: 'An unexpected error occurred.' };
      }
    };
    