const API_BASE_URL = 'https://localhost:7198'

export default API_BASE_URL;

// Auth-related endpoints
export const AUTH_API = {
  LOGIN: `${API_BASE_URL}/api/users/login`,
  CHANGE_PASSWORD: `${API_BASE_URL}/api/users/reset-password`,
  VERIFY_OTP: `${API_BASE_URL}/api/users/verify-otp`,
  REGISTER: `${API_BASE_URL}/api/users/register`,
  REQUEST_PASSWORD_RESET: `${API_BASE_URL}/api/users/request-password-reset`,
  
};

export const USER_API = {
  
    GET_USER: (userId: number) => `${API_BASE_URL}/api/users/${userId}/profile`,
    GET_CURRENT_USER: `${API_BASE_URL}/api/users/profile`,
    UPDATE_USER: `${API_BASE_URL}/api/users/profile`,
    SEARCH: `${API_BASE_URL}/api/users/search`,
};

export const POST_API = {

    GET_USER_POSTS: (userId: number) => `${API_BASE_URL}/api/posts/user-posts/${userId}`,
    GET_FEED: `${API_BASE_URL}/api/posts/activity-feed`,
    GET_POST: (postId: number) => `${API_BASE_URL}/api/posts/${postId}`,
    CREATE_POSTS: `${API_BASE_URL}/api/posts`,

};

export const COMMENT_API = {

    CREATE_COMMENT: `${API_BASE_URL}/api/comments`,
    GET_COMMENTS: (postId: number) => `${API_BASE_URL}/api/comments/post/${postId}`,

};

export const FRIEND_API = {

    SEND_REQUEST: `${API_BASE_URL}/api/friendships/send-request`,
    ACCEPT_REQUEST: (friendId: number) => `${API_BASE_URL}/api/friendships/accept/${friendId}`,

};

export const LIKE_API = {

    SEND_LIKE: `${API_BASE_URL}/api/likes`,

};

export const NOTIFICATION_API = {

    GET_NOTIFICATIONS: `${API_BASE_URL}/api/notifications`,

};

export const MESSAGE_API = {

    GET_CONVERSATION: (friendId: number) => `${API_BASE_URL}/api/messages/conversation/${friendId}`,
    SEND_MESSAGE: `${API_BASE_URL}/api/messages`,

};


