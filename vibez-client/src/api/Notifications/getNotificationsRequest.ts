import { NOTIFICATION_API } from '../apiConsts.ts';

export const getNotificationsRequest = async (token: string) => {
  try {
    const response = await fetch(NOTIFICATION_API.GET_NOTIFICATIONS, {
      method: "GET",
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      },
    });

    if (response.ok) {
      console.log("Notifications fetched successfully");
      const data = await response.json();
      return { success: true, data };
    } else {
      return { success: false, error: `Failed to fetch notifications. Status: ${response.status}` };
    }
  } catch (error) {
    console.error("Error fetching notifications:", error);
    return { success: false, error: error.message || "Unknown error occurred." };
  }
};
