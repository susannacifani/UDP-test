#define _WINSOCK_DEPRECATED_NO_WARNINGS
#include <stdio.h>
#include <winsock2.h>

#pragma comment(lib, "ws2_32.lib")

#define SERVER_PORT 5150
#define UNITY_PORT 5052
#define BUFFER_SIZE 1024

int main() {
    WSADATA wsa;
    SOCKET client_socket;
    struct sockaddr_in server_addr, unity_addr;
    char buffer[BUFFER_SIZE];
    int server_addr_len = sizeof(server_addr);

    // Inizializzazione Winsock
    printf("Inizializzazione di Winsock...\n");
    if (WSAStartup(MAKEWORD(2, 2), &wsa) != 0) {
        printf("Errore di inizializzazione: %d\n", WSAGetLastError());
        return 1;
    }

    // Creazione socket UDP
    if ((client_socket = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP)) == INVALID_SOCKET) {
        printf("Errore nella creazione del socket: %d\n", WSAGetLastError());
        WSACleanup();
        return 1;
    }

    server_addr.sin_family = AF_INET;
    server_addr.sin_addr.s_addr = INADDR_ANY;
    server_addr.sin_port = htons(SERVER_PORT);

    if (bind(client_socket, (struct sockaddr*)&server_addr, sizeof(server_addr)) == SOCKET_ERROR) {
        printf("Errore nel bind del socket: %d\n", WSAGetLastError());
        closesocket(client_socket);
        WSACleanup();
        return 1;
    }

    unity_addr.sin_family = AF_INET;
    unity_addr.sin_addr.s_addr = inet_addr("127.0.0.1"); // Inoltra a Unity su localhost
    unity_addr.sin_port = htons(UNITY_PORT);

    printf("Client in ascolto sulla porta %d e inoltra su %d...\n", SERVER_PORT, UNITY_PORT);


    while (1) {
        int recv_size = recvfrom(client_socket, buffer, sizeof(buffer) - 1, 0, (struct sockaddr*)&server_addr, &server_addr_len);
        if (recv_size == SOCKET_ERROR) {
            printf("Errore nella ricezione: %d\n", WSAGetLastError());
            break;
        } else {
            buffer[recv_size] = '\0'; 
            // printf("Ricevuto dal server: %s\n", buffer);

            // Invia a Unity sulla porta 5052
            if (sendto(client_socket, buffer, recv_size, 0, (struct sockaddr*)&unity_addr, sizeof(unity_addr)) == SOCKET_ERROR) {
                printf("Errore nell'inoltro a Unity: %d\n", WSAGetLastError());
                break;
            } else {
                printf(buffer);
            }
        }
    }

    closesocket(client_socket);
    WSACleanup();

    return 0;
}
