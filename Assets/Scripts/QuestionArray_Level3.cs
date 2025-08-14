using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestionArray_Level3 : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI answer1Text;
    public TextMeshProUGUI answer2Text;
    public TextMeshProUGUI answer3Text;
    public TextMeshProUGUI explanationText;

    string[] questions = new string[]
    {
        // T-1
        "____ is a connection-oriented protocol.",
        // T-2
        "____ is a connection-less protocol.",
        // T-3
        "TCP stands for Transmission ____ Protocol.",
        // T-4
        "The port number for Telnet is ____",
        // T-5
        "The port number for HTTP is ____",
        // T-6
        "Port number ____ is used for Telnet application.",
        // T-7
        "All transport layer protocols are standardized by ____",
        // T-8
        "Which one is the function of transport layer?",
        // T-9
        "The data unit used in TCP is known as ____",
        // T-10
        "A client process sends ____ to initiate a TCP session.",
        // T-11
        "In three-way handshake, after received a SYN, a server sends ____ to the client.",
        // T-12
        "Choose the correct sequence for three-way handshake in TCP.",
        // T-13
        "A client process sends ____ to terminate  a TCP session.",
        // T-14
        "In TCP header, the ____ field identifies a TCP segment.",
        // T-15
        "UDP stands for User ____ Protocol.",
        // T-16
        "This field in NOT in a UDP header.",
        // T-17
        "Which field in TCP header that control the amount of data sent by the sender?",
        // T-18
        "FTP uses ____ as its transport protocol.",
        // T-19
        "Which one is the transport layer protocol?",
        // T-20
        "The most suitable transport protocol for video transmission is ____",
        // T-21
        "Port number ____ is used for data transfer in FTP.",
        // T-22
        "The ____ field in TCP header is used for checking the possibility of duplication or non-orderly delivery of segments.",
        // T-23
        "____ is beneficial to delay sensitive data.",
        // T-24
        "This field must be part of all transport protocol's header.",
        // T-25
        "This is NOT the function of transport layer.",
        // T-26
        "DNS uses ____ as its transport protocol.",
        // T-27
        "Choose the correct application that uses UDP.",
        // T-28
        "The port range of ____ is known as well-known ports.",
        // T-29
        "The port range of ____ is known as well-known ports.",
        // T-30
        "____ is the organization that maintains the allocation of port numbers in the Internet.",
        // T-31
        "Port number is used to identify ____",
        // T-32
        "This flag is NOT used for congestion control in TCP header.",
        // T-33
        "A TCP connection is established after a ____ way handshake.",
        // T-34
        "When the window size is 0 at the client side, this indicates that ____",
        // T-35
        "The data unit used in UDP is known as ____",
        // T-36
        "The size of UDP header is ____ bytes which is less than TCP header.",
        // T-37
        "The lack of sequence number in ____ means that delivery of data is likely to be received at the destination in an incorrect and non-sequenced order.",
        // T-38
        "FTP server receives a connection request from a client via port ____",
        // T-39
        "DHCP uses ____ as its transport protocol.",
        // T-40
        "Port number ____ is used for HTTPS.",
        // T-41
        "Network Time Protocol uses ____ as its transport protocol.",
        // T-42
        "The ____ field in TCP header is used for error-check of the header and data.",
        // T-43
        "Telnet uses ____ as its transport protocol.",
        // T-44
        "Which transport layer protocol suitable for email transmission?",
        // T-45
        "Port number ____ is used for DNS application.",
        // T-46
        "The length of port number in TCP header is ____ bits.",
        // T-47
        "____ layer is located in between the Application and Network layer.",
        // T-48
        "The pair of ____ and IP address is also known as socket address.",
        // T-49
        "In OSI model, transport layer is located in Layer ____",
        // T-50
        "This protocol uses TCP as its transport layer protocol.",
        // T-51
        "Which protocol is used in the transport layer?",
        // T-52
        "What is the function of flow control in transport layer?",
        // T-53
        "Which protocol is used for online transaction?",
        // T-54
        "Which is the correct TCP port number for related protocols?"
    };

    string[][] answers = new string[][]
    {
        new string[] { "UDP", "ARP", "TCP" }, // T-1
        new string[] { "TCP", "SCTP", "UDP" }, // T-2
        new string[] { "Communication", "Connection", "Control" }, // T-3
        new string[] { "21", "22", "23" }, // T-4
        new string[] { "81", "443", "80" }, // T-5
        new string[] { "22", "25", "23" }, // T-6
        new string[] { "EIA", "IEEE", "IETF" }, // T-7
        new string[] { "Provides an interface for user application", "Transmit packet from source to destintion", "Establish and maintain the connection of end-to-end process" }, // T-8
        new string[] { "Frame", "Datagram", "Segment" }, // T-9
        new string[] { "ACK", "SYN+ACK", "SYN" }, // T-10
        new string[] { "FIN", "SYN", "SYN+ACK" }, // T-11
        new string[] { "FIN --> FIN+ACK --> ACK", "ACK --> SYN+ACK --> SYN", "SYN --> SYN+ACK --> ACK" }, // T-12
        new string[] { "SYN", "PSH", "FIN" }, // T-13
        new string[] { "Source port", "Header length", "Sequence number" }, // T-14
        new string[] { "Delivery", "Destination", "Datagram" }, // T-15
        new string[] { "UDP length", "Source port", "Window size" }, // T-16
        new string[] { "Destination port", "Checksum", "Window size" }, // T-17
        new string[] { "SCTP", "UDP", "TCP" }, // T-18
        new string[] { "SMTP", "HTTP", "TCP" }, // T-19
        new string[] { "IMAP", "TCP", "UDP" }, // T-20
        new string[] { "22", "21", "20" }, // T-21
        new string[] { "Destination port", "Header Length", "Acknowlegment number" }, // T-22
        new string[] { "DCCP", "SCTP", "UDP" }, // T-23
        new string[] { "Acknowlegment number", "Flag", "Destination port" }, // T-24
        new string[] { "Identify the port number of network application", "Initiate connection with a server application", "Forward packet based on the IP address" }, // T-25
        new string[] { "WWW", "RTP", "UDP" }, // T-26
        new string[] { "SMTP and IMAP", "HTTP and HTTPS", "DHCP and DNS" }, // T-27
        new string[] { "65536 - 70000", "1024 - 49151", "0 - 1023" }, // T-28
        new string[] { "1024 - 49151", "0 - 1023", "49152 - 65535" }, // T-29
        new string[] { "IEEE", "EIA", "IANA" }, // T-30
        new string[] { "subnet", "network", "process" }, // T-31
        new string[] { "CWR", "ECE", "SYN" }, // T-32
        new string[] { "one", "five", "three" }, // T-33
        new string[] { "Client initiate a connection request", "Client connection is terminated", "Client suspending the receiving of TCP data segment temporarily" }, // T-34
        new string[] { "segment", "packet", "datagram" }, // T-35
        new string[] { "40", "20", "8" }, // T-36
        new string[] { "SCTP", "TCP", "UDP" }, // T-37
        new string[] { "22", "23", "21" }, // T-38
        new string[] { "RTP", "TCP", "UDP" }, // T-39
        new string[] { "80", "433", "443" }, // T-40
        new string[] { "SCTP", "DNS", "UDP" }, // T-41
        new string[] { "Source port", "Option", "Checksum" }, // T-42
        new string[] { "UDP", "MTP", "TCP" }, // T-43
        new string[] { "UDP", "PPP", "TCP" }, // T-44
        new string[] { "80", "25", "53" }, // T-45
        new string[] { "32", "24", "16" }, // T-46
        new string[] { "Physical", "Datalink", "Transport" }, // T-47
        new string[] { "URL", "MAC address", "Port number" }, // T-48
        new string[] { "2", "3", "4" }, // T-49
        new string[] { "DHCP", "SNMP", "BGP" }, // T-50
        new string[] { "SMTP", "FTP", "TCP" }, // T-51
        new string[] { "Provide error detection", "Ensuring data integrity", "Managing network congestion" }, // T-52
        new string[] { "TELNET", "HTTP", "HTTPS" }, // T-53
        new string[] { "HTTPS - port 442", "DNS - port 52", "SSH - port 22" } // T-54
    };

    string[] explanations = new string[]
    {
        "TCP (Transmission Control Protocol) establishes reliable, connection-oriented communication between devices, ensuring data delivery and integrity.", // T-1
        "UDP (User Datagram Protocol) is a connectionless protocol that provides faster but unreliable data transmission, suitable for real-time applications.", // T-2
        "TCP stands for Transmission Control Protocol, emphasizing its role in controlling data transmission over networks.", // T-3
        "The Telnet service uses port number '23' for communication between the client and server.", // T-4
        "HTTP (Hypertext Transfer Protocol) utilizes port number '80' for web page retrieval and communication.", // T-5
        "Port number '23' is used by the Telnet application to establish connections.", // T-6
        "Transport layer protocols like TCP and UDP are standardized by the Internet Engineering Task Force (IETF), ensuring interoperability.", // T-7
        "The Transport layer establishes and manages end-to-end connections, including data sequencing and flow control.", // T-8
        "In TCP, the data unit is called a 'segment,' which encapsulates the data for transmission.", // T-9
        "A client process initiates a TCP session by sending a SYN (Synchronize) packet.", // T-10
        "After receiving a SYN packet, a server responds with a SYN+ACK (Synchronize-Acknowledge) packet during the three-way handshake.", // T-11
        "The correct sequence for a three-way handshake in TCP is: SYN --> SYN+ACK --> ACK.", // T-12
        "A client process sends a FIN (Finish) packet to terminate a TCP session.", // T-13
        "The 'Sequence Number' field in the TCP header identifies the sequence of TCP segments for reordering at the destination.", // T-14
        "UDP stands for User Datagram Protocol, emphasizing its focus on connectionless, datagram-oriented communication.", // T-15
        "The 'Window Size' field is not present in the UDP header, as UDP does not utilize flow control mechanisms like TCP.", // T-16
        "The 'Window Size' field in TCP header controls the amount of data sent by the sender, allowing for flow control.", // T-17
        "FTP (File Transfer Protocol) utilizes TCP as its transport protocol for secure and reliable file transfer.", // T-18
        "TCP (Transmission Control Protocol) is a transport layer protocol responsible for reliable data delivery and connection management.", // T-19
        "UDP (User Datagram Protocol) is suitable for video transmission due to its speed and tolerance for some data loss.", // T-20
        "The port number '20' is used for data transfer in the FTP (File Transfer Protocol) application.", // T-21
        "The 'Acknowledgment Number' field in the TCP header checks for the possible duplication or non-sequential delivery of segments.", // T-22
        "UDP (User Datagram Protocol) is advantageous for delay-sensitive data as it focuses on speed over reliability.", // T-23
        "The 'Destination Port' field must be included in all transport protocol headers to direct data to the correct process.", // T-24
        "The Transport layer does not forward packets based on IP addresses; that is a function of the Network layer.", // T-25
        "DNS (Domain Name System) uses UDP as its transport protocol for fast name resolution.", // T-26
        "DHCP (Dynamic Host Configuration Protocol) and DNS (Domain Name System) are applications utilizing UDP for their communication.", // T-27
        "The port range '0 - 1023' is known as well-known ports, reserved for standard services and protocols.", // T-28
        "The port range '49152 - 65535' is known as dynamic or private ports, used for client-side communication.", // T-29
        "The Internet Assigned Numbers Authority (IANA) manages and allocates port numbers in the Internet.", // T-30
        "Port numbers are used to identify processes running on devices within a network.", // T-31
        "The SYN (Synchronize) flag in TCP is used to initiate a connection, not for congestion control.", // T-32
        "A TCP connection is established through a three-way handshake involving SYN, SYN+ACK, and ACK packets.", // T-33
        "A window size of 0 at the client side indicates a temporary suspension of receiving TCP data segments.", // T-34
        "UDP uses 'datagrams' as its data unit, which encapsulates the data for transmission.", // T-35
        "The UDP header size is 8 bytes, smaller than the TCP header size, optimizing UDP for speed.", // T-36
        "The absence of a sequence number in UDP leads to a higher likelihood of receiving data out of order or with duplication.", // T-37
        "FTP servers receive connection requests from clients via port number '21' for control and initial communication.", // T-38
        "DHCP (Dynamic Host Configuration Protocol) uses UDP for fast and automatic IP address configuration on networks.", // T-39
        "HTTPS (Hypertext Transfer Protocol Secure) uses port number '443' for secure web communication.", // T-40
        "The Network Time Protocol (NTP) employs UDP for clock synchronization and time management across networks.", // T-41
        "The 'Checksum' field in the TCP header verifies the integrity of both the header and data during transmission.", // T-42
        "Telnet, a remote access protocol, uses TCP as its transport protocol for command transmission.", // T-43
        "TCP (Transmission Control Protocol) is suitable for email transmission due to its reliability in ensuring data delivery.", // T-44
        "The DNS (Domain Name System) application uses port number '53' for domain name resolution.", // T-45
        "The length of the port number in the TCP header is 16 bits, allowing for a range of 0 to 65535.", // T-46
        "The Transport layer in the OSI model resides between the Application and Network layers, managing end-to-end communication.", // T-47
        "A socket address comprises the pair of a port number and an IP address, uniquely identifying a process on a network.", // T-48
        "In the OSI model, the Transport layer is positioned at Layer 4, providing end-to-end communication services.", // T-49
        "BGP (Border Gateway Protocol) relies on TCP as its transport layer protocol for reliable and controlled data transfer.", // T-50
        "TCP or Transmission Control Protocol is used in transport layer for reliable and connection-oriented communication. SMTP and FTP operate in the application layer.", // T-51
        "Flow control in the transport layer manages network congestion by controlling the rate of data transmission between sender and receiver to avoid flooding.", // T-52
        "HTTPS is used for online transaction due to its secure and encrypted nature which provides confidentiality and integrity of sensitive information, while both TELNET and HTTP transmit data in plain text only.", // T-53
        "SSH using port 22, DNS using port 53 while HTTPS using port 443." // T-54
    };

    List<int> shuffledIndexes = new List<int>();
    int currentQuestionIndex = 0;

    void Start()
    {
        ShuffleQuestions();
        DisplayNextQuestion();
    }

    void ShuffleQuestions()
    {
        shuffledIndexes.Clear();
        for (int i = 0; i < questions.Length; i++)
        {
            shuffledIndexes.Add(i);
        }
        for (int i = 0; i < shuffledIndexes.Count; i++)
        {
            int temp = shuffledIndexes[i];
            int randomIndex = Random.Range(i, shuffledIndexes.Count);
            shuffledIndexes[i] = shuffledIndexes[randomIndex];
            shuffledIndexes[randomIndex] = temp;
        }
    }

    void DisplayNextQuestion()
    {
        if (currentQuestionIndex >= shuffledIndexes.Count)
        {
            currentQuestionIndex = 0;
            ShuffleQuestions();
        }

        if (shuffledIndexes.Count == 0)
        {
            Debug.LogError("No questions available.");
            return;
        }

        int index = shuffledIndexes[currentQuestionIndex];

        questionText.text = questions[index];

        string[] selectedAnswers = answers[index];
        string explanation = explanations[index];

        if (selectedAnswers.Length >= 3)
        {
            answer1Text.text = selectedAnswers[0];
            answer2Text.text = selectedAnswers[1];
            answer3Text.text = selectedAnswers[2];
        }
        else
        {
            Debug.LogError("Not enough answer: " + questions[index]);
        }

        explanationText.text = explanation;
        currentQuestionIndex++;
    }
}