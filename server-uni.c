#define WIN32_LEAN_AND_MEAN
#include <winsock2.h>
#include <ws2tcpip.h>
#include <stdio.h>
#include <windows.h>

#pragma comment(lib, "Ws2_32.lib")

#define SERVER_PORT 5150
#define BUFFER_SIZE 512

int main() {
    WSADATA wsaData;
    SOCKET serverSocket;
    struct sockaddr_in serverAddr;
    char buffer[BUFFER_SIZE];

    // inizializza Winsock
    if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0) {
        printf("Errore nella inizializzazione di Winsock.\n");
        return 1;
    }

    // crea socket 
    serverSocket = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
    if (serverSocket == INVALID_SOCKET) {
        printf("Errore nella creazione del socket: %d\n", WSAGetLastError());
        WSACleanup();
        return 1;
    }

    serverAddr.sin_family = AF_INET;
    serverAddr.sin_addr.s_addr = inet_addr("10.90.71.142"); // IP locale
    serverAddr.sin_port = htons(SERVER_PORT);

    printf("Server attivo sulla porta %d...\n", SERVER_PORT);

    FILE *pipe = popen("generator.exe", "r");
    if (pipe == NULL) {
        printf("Errore nell'aprire la pipe.\n");
        closesocket(serverSocket);
        WSACleanup();
        return 1;
    }

    // inizia a inviare le stringhe 
    while (1) {
        if (fgets(buffer, BUFFER_SIZE, pipe) != NULL) {
            struct sockaddr_in clientAddr;
            clientAddr.sin_family = AF_INET;
            clientAddr.sin_port = htons(SERVER_PORT);
            clientAddr.sin_addr.s_addr = inet_addr("10.90.255.255"); // Broadcast su rete locale

            if (sendto(serverSocket, buffer, (int)strlen(buffer), 0, (struct sockaddr*)&clientAddr, sizeof(clientAddr)) == SOCKET_ERROR) {
                printf("Errore nell'invio del messaggio: %d\n", WSAGetLastError());
            } else {
                printf("Inviato al client: %s", buffer);
            }
        } else {
            printf("Errore nella lettura dalla pipe.\n");
            break;
        }
        Sleep(1000);
    }

    pclose(pipe);
    closesocket(serverSocket);
    WSACleanup();

    return 0;
}
