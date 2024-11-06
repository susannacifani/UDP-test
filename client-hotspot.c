#define _WINSOCK_DEPRECATED_NO_WARNINGS
#include <stdio.h>
#include <winsock2.h>

#pragma comment(lib, "ws2_32.lib")

int main() {
    WSADATA wsa;
    SOCKET client_socket;
    struct sockaddr_in server_addr;
    char buffer[1024];
    int server_addr_len = sizeof(server_addr);

    // inizializzazione Winsock
    printf("Inizializzazione di Winsock...\n");
    if (WSAStartup(MAKEWORD(2, 2), &wsa) != 0) {
        printf("Errore di inizializzazione: %d\n", WSAGetLastError());
        return 1;
    }

    // creazione socket
    if ((client_socket = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP)) == INVALID_SOCKET) {
        printf("Errore nella creazione del socket: %d\n", WSAGetLastError());
        WSACleanup();
        return 1;
    }

    // Configurazione dell'indirizzo del server
    server_addr.sin_family = AF_INET;
    server_addr.sin_addr.s_addr = INADDR_ANY;
    server_addr.sin_port = htons(5150); // porta server


    if (bind(client_socket, (struct sockaddr*)&server_addr, sizeof(server_addr)) == SOCKET_ERROR) {
        printf("Errore nel bind del socket: %d\n", WSAGetLastError());
        closesocket(client_socket);
        WSACleanup();
        return 1;
    }

    printf("Client in ascolto sulla porta %d...\n", 5150);

    while (1) {
        int recv_size = recvfrom(client_socket, buffer, sizeof(buffer) - 1, 0, (struct sockaddr*)&server_addr, &server_addr_len);
        if (recv_size == SOCKET_ERROR) {
            printf("Errore nella ricezione: %d\n", WSAGetLastError());
            break;
        } else {
            buffer[recv_size] = '\0';
            printf(buffer);
        }
    }

    closesocket(client_socket);
    WSACleanup();

    return 0;
}
