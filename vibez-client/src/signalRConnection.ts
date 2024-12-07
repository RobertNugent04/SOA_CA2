import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";

/**
 * Creates and returns a SignalR Hub connection instance.
 * @param token The user's authentication token for secure communication.
 * @returns A SignalR Hub connection instance.
 */
export const createNotificationHubConnection = (token: string) => {
  const connection = new HubConnectionBuilder()
    .withUrl(
      `https://vibez-web-service-g8gzbmfvdnc2hahw.northeurope-01.azurewebsites.net/notificationHub`,
      {
        accessTokenFactory: () => token, // Use token for authentication
      }
    )
    .configureLogging(LogLevel.Information) // Optional: Set logging level
    .withAutomaticReconnect() // Automatically reconnect on disconnection
    .build();

  return connection;
};
