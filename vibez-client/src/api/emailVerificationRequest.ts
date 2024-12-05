const endpoint = 'https://localhost:7198/api/users/verify-otp';

interface EmailVerificationPayload {
  email: string;
  otp: string;
}

export const emailVerificationRequest = async (payload: EmailVerificationPayload) => {
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
      console.log('OTP verification successful:', data);
      return data; // Return success data
    } else {
      const errorData = await response.json();
      console.error('OTP verification error:', errorData);
      return { success: false, message: 'OTP verification failed.', error: errorData };
    }
  } catch (error) {
    console.error('Error during OTP verification request:', error);
    return { success: false, message: 'An error occurred while verifying the OTP.', error };
  }
};
