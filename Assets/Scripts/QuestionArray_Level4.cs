using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestionArray_Level4 : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI answer1Text;
    public TextMeshProUGUI answer2Text;
    public TextMeshProUGUI answer3Text;
    public TextMeshProUGUI explanationText;

    string[] questions = new string[]
    {
        // A-1
        "TFTP stands for ____ File Transfer Protocol.",
        // A-2
        "____ is designed to transfer small files and is easier to implement.",
        // A-3
        "Port number ____ is used for TFTP application.",
        // A-4
        "TFTP uses ____ as its transport protocol.",
        // A-5
        "Which port is used by email application ?",
        // A-6
        "DNS stands for Domain ____ System.",
        // A-7
        "You can use ____ for remote login to the Access Points (AC), routers and switches.",
        // A-8
        "HTTP stands for Hyper Text ____ Protocol.",
        // A-9
        "SNMP stands for Simple Network ____ Protocol.",
        // A-10
        "____ is designed to dynamically assign suitable IP address to hosts.",
        // A-11
        "DHCP client uses port ____ for connection to a DHCP server.",
        // A-12
        "DHCP server uses port ____ to receive a request from DHCP client.",
        // A-13
        "DHCP stands for Dynamic Host ____ Protocol.",
        // A-14
        "____ uniquely identifies the location of a Web page or other resources in the Internet.",
        // A-15
        "____ language is used for displaying Web content in a browser.",
        // A-16
        "____ protocol is used to transfer Web content in the Internet.",
        // A-17
        "The protocol used for resolving domain name to IP address is ____",
        // A-18
        "HTML stands for Hyper Text ____ Language.",
        // A-19
        "This is a top level domain (TLD) for www.upm.edu.my.",
        // A-20
        "The top level domain of www.example.com is ____",
        // A-21
        "____ is an application layer protocol that can synchronize the clocks of devices in the Internet.",
        // A-22
        "Port number ____ is used for NTP application.",
        // A-23
        "NTP stands for Network ____ Protocol.",
        // A-24
        "SNMP, NETCONF and Telemety are used for ____ application.",
        // A-25
        "These ports are used for SNMP.",
        // A-26
        "SNMP uses ____ as its transport protocol.",
        // A-27
        "NETCONF uses ____ as its transport protocol.",
        // A-28
        "Yang is data modeling language used in NETCONF. True of False?",
        // A-29
        "____ is a database containing variables for the managed devices in SNMP.",
        // A-30
        "____ is a transition technology that assign a private IP address to a public IP address to overcome the IPv4 address shortage.",
        // A-31
        "This type of NAT assign both the IP address and port number from multiple internal host into one public IP address in an address pool.",
        // A-32
        "____ is a security framework to manage the user's account, username and password.",
        // A-33
        "Which of the following server implement AAA?",
        // A-34
        "____ is a set of rules to permit or deny access to resources.",
        // A-35
        "The ACL number that range from 2000-3000 is within the ____ category.",
        // A-36
        "The core of ____ is to separate the control plane from the data plane of network devices and implements centralized control.",
        // A-37
        "AAA is an abbreviation for Authentication, ____ and Accounting.",
        // A-38
        "The ____ in AAA determines which users can access the network.",
        // A-39
        "The RADIUS and TACACS Server implement ____ security architecture.",
        // A-40
        "HTTP and ____ applications use TCP as its transport protocol.",
        // A-41
        "DNS and ____ applications use UDP as its transport protocol.",
        // A-42
        "This application is used to securely remote login to a server.",
        // A-43
        "____ protocol is used for sending email in the Internet.",
        // A-44
        "____ is used to extend the format of email messages for sending and receiving audio, video and images attached to the email.",
        // A-45
        "This is NOT a top level domain.",
        // A-46
        "The highest level of name server in the hierarchy of DNS is ____ server.",
        // A-47
        "This record contains the IPv4 address of a domain name.",
        // A-48
        "Which of the following protocol is an application layer protocol?",
        // A-49
        "The ____ layer provides interface for application software so that application can use the services.",
        // A-50
        "This is the highest layer in TCP/IP protocol suite.",
        // A-51
        "Which protocol is used for sending an email?",
        // A-52
        "What is the function of DNS in the application layer?",
        // A-53
        "Which is the correct port number for related protocols?",
        // A-54
        "Which protocol is NOT used in electronic mail management?",
        // A-55
        "What is the function of SNMP in the application layer?"
    };

    string[][] answers = new string[][]
    {
        new string[] { "Transport", "Top", "Trivial" }, // A-1
        new string[] { "HTTP", "SMTP", "TFTP" }, // A-2
        new string[] { "80", "25", "69" }, // A-3
        new string[] { "TCP", "RTP", "UDP" }, // A-4
        new string[] { "35", "45", "25" }, // A-5
        new string[] { "Notification", "New", "Name" }, // A-6
        new string[] { "Web", "Email", "Telnet" }, // A-7
        new string[] { "Transit", "Transmission", "Transfer" }, // A-8
        new string[] { "Mail", "Maintenance", "Management" }, // A-9
        new string[] { "SMTP", "SNMP", "DHCP" }, // A-10
        new string[] { "66", "67", "68" }, // A-11
        new string[] { "21", "68", "67" }, // A-12
        new string[] { "Control", "Connection", "Configuration" }, // A-13
        new string[] { "FTP", "DNS", "URL" }, // A-14
        new string[] { "DNS", "UDP", "HTML" }, // A-15
        new string[] { "TCP", "DHCP", "HTTP" }, // A-16
        new string[] { "SMTP", "WWW", "DNS" }, // A-17
        new string[] { "Model", "Management", "Markup" }, // A-18
        new string[] { "upm", "edu", "my" }, // A-19
        new string[] { "www", "example", "com" }, // A-20
        new string[] { "FTP", "HTTP", "NTP" }, // A-21
        new string[] { "68", "8080", "123" }, // A-22
        new string[] { "Transfer", "Transmission", "Time" }, // A-23
        new string[] { "file transfer", "video transfer", "network management" }, // A-24
        new string[] { "67 and 68", "25 and 26", "161 and 162" }, // A-25
        new string[] { "DHCP", "DNS", "UDP" }, // A-26
        new string[] { "UDP", "DNS", "TCP" }, // A-27
        new string[] { "Not Sure", "False", "True" }, // A-28
        new string[] { "DNS", "NMS", "MIB" }, // A-29
        new string[] { "AAAA", "AAA", "NAT" }, // A-30
        new string[] { "ACL", "EasyIP", "NAPT" }, // A-31
        new string[] { "A", "ACL", "AAA" }, // A-32
        new string[] { "DNS", "WWW", "Radius" }, // A-33
        new string[] { "SSL", "TLS", "ACL" }, // A-34
        new string[] { "Advanced ACL", "Admin ACL", "Basic ACL" }, // A-35
        new string[] { "FTP", "DHCP", "SDN" }, // A-36
        new string[] { "Accessibility", "Availability", "Authorization" }, // A-37
        new string[] { "Accounting", "Authorization", "Authentication" }, // A-38
        new string[] { "A", "MX", "AAA" }, // A-39
        new string[] { "DHCP", "SNMP", "Telnet" }, // A-40
        new string[] { "FTP", "SMTP", "DHCP" }, // A-41
        new string[] { "Email", "DNS", "SSH" }, // A-42
        new string[] { "Telnet", "IMAP", "SMTP" }, // A-43
        new string[] { "DNS", "AAAA", "MIME" }, // A-44
        new string[] { ".com", ".org", ".dridawaty" }, // A-45
        new string[] { "authoritative", "TLD", "root" }, // A-46
        new string[] { "MX", "AAAA", "A" }, // A-47
        new string[] { "TCP", "ARP", "HTTP" }, // A-48
        new string[] { "Transport", "Network", "Application" }, // A-49
        new string[] { "Datalink", "Transport", "Application" }, // A-50
        new string[] { "IMAP4", "POP3", "SMTP" }, // A-51
        new string[] { "Transferring files between servers", "Assigning IP addresses to devices", "Resolving domain names to IP addresses" }, // A-52
        new string[] { "FTP - 23", "BGP - 178", "SNMP - 161" }, // A-53
        new string[] { "IMAP4", "SMTP", "SFTP" }, // A-54
        new string[] { "Retrieving web pages and resources", "Accessing network remotely", "Monitoring network devices" } // A-55
    };

    string[] explanations = new string[]
    {
        "TFTP (Trivial File Transfer Protocol) is designed for simple and lightweight file transfers, particularly suited for small file exchanges.", // A-1
        "TFTP is specifically designed for small file transfers due to its simplicity and ease of implementation.", // A-2
        "TFTP uses port number '69' for communication between the client and server.", // A-3
        "TFTP utilizes UDP (User Datagram Protocol) as its transport protocol for faster but less reliable data transmission.", // A-4
        "The Email application uses port number '25' for Simple Mail Transfer Protocol (SMTP) communication.", // A-5
        "DNS (Domain Name System) is responsible for translating domain names into IP addresses.", // A-6
        "Telnet is an application that enables remote login to devices like Access Points (ACs), routers, and switches.", // A-7
        "HTTP (Hypertext Transfer Protocol) facilitates the transfer of hypertext or web pages on the internet.", // A-8
        "SNMP (Simple Network Management Protocol) is used for managing and monitoring network devices.", // A-9
        "DHCP (Dynamic Host Configuration Protocol) dynamically assigns suitable IP addresses to network hosts.", // A-10
        "The DHCP client uses port number '68' for connections to DHCP servers.", // A-11
        "The DHCP server uses port number '67' to receive requests from DHCP clients.", // A-12
        "DHCP stands for Dynamic Host Configuration Protocol, emphasizing its role in configuring network hosts.", // A-13
        "A URL (Uniform Resource Locator) uniquely identifies the location of resources like web pages on the internet.", // A-14
        "HTML (Hypertext Markup Language) is used for creating and displaying web content in a browser.", // A-15
        "HTTP (Hypertext Transfer Protocol) is used to transfer web content, including text, images, and other media.", // A-16
        "The DNS protocol is used for resolving domain names to their corresponding IP addresses.", // A-17
        "HTML stands for HyperText Markup Language, used for structuring and presenting web content.", // A-18
        "The top-level domain (TLD) for www.upm.edu.my is 'my', representing Malaysia.", // A-19
        "The top-level domain of www.example.com is 'com', representing commercial entities.", // A-20
        "NTP (Network Time Protocol) synchronizes device clocks across the internet.", // A-21
        "NTP uses port number '123' for synchronization between network devices.", // A-22
        "NTP stands for Network Time Protocol, focusing on time synchronization across networks.", // A-23
        "SNMP, NETCONF, and Telemetry are used for managing and monitoring network devices and infrastructure.", // A-24
        "The ports used for SNMP communication are '161' for SNMP requests and '162' for SNMP traps.", // A-25
        "SNMP uses UDP (User Datagram Protocol) for communication between the SNMP manager and agents.", // A-26
        "NETCONF uses TCP (Transmission Control Protocol) as its transport protocol for secure device configuration.", // A-27
        "YANG is indeed a data modeling language used in NETCONF for configuring network devices.", // A-28
        "MIB (Management Information Base) contains variables used by managed devices in SNMP.", // A-29
        "NAT (Network Address Translation) is a transitional technology for conserving IPv4 addresses.", // A-30
        "NAPT (Network Address Port Translation) assigns both IP addresses and port numbers from internal hosts to one public IP address.", // A-31
        "AAA (Authentication, Authorization, and Accounting) manages user accounts and permissions in a network.", // A-32
        "RADIUS is an AAA server used for centralizing authentication, authorization, and accounting.", // A-33
        "ACL (Access Control List) is a set of rules that permits or denies access to network resources.", // A-34
        "Basic ACLs are numbered from 2000 to 3000 and are used for simple packet filtering.", // A-35
        "SDN (Software-Defined Networking) separates the control plane from the data plane in network devices.", // A-36
        "AAA stands for Authentication, Authorization, and Accounting, key aspects in managing user access.", // A-37
        "Authorization in AAA determines which users have access rights to resources within the network.", // A-38
        "RADIUS and TACACS+ are servers implementing AAA security architecture.", // A-39
        "HTTP and Telnet applications utilize TCP (Transmission Control Protocol) for reliable communication.", // A-40
        "DNS and DHCP applications use UDP (User Datagram Protocol) for faster, connectionless communication.", // A-41
        "SSH (Secure Shell) is a secure remote login protocol used for secure communication with servers.", // A-42
        "SMTP (Simple Mail Transfer Protocol) is used for sending emails over the internet.", // A-43
        "MIME (Multipurpose Internet Mail Extensions) extends the format of email messages to include multimedia attachments.", // A-44
        ".dridawaty is not a recognized top-level domain (TLD) in the domain name system.", // A-45
        "The root server in the DNS hierarchy is the highest-level name server responsible for the root zone.", // A-46
        "A records in DNS contain IPv4 addresses corresponding to domain names.", // A-47
        "HTTP (Hypertext Transfer Protocol) is an application layer protocol used for transmitting web content.", // A-48
        "The Application layer provides interfaces for application software, enabling them to utilize network services.", // A-49
        "The Application layer is the highest layer in the TCP/IP protocol suite, facilitating end-user applications.", // A-50
        "SMTP or Simple Mail Transfer Protocol is used for sending email as it focuses on the transmission of outgoing messages. POP3 and IMAP4 are used to retrieve incoming emails from a server.", // A-51
        "DNS or Domain Name System resolves domain names to IP addresses which allowing users to access resources over the Internet using human-readable names instead of numerical IP addresses.", // A-52
        "SNMP using port 161, BGP using port 179 and FTP using port 20 for data transmission and port 21 for file control.", // A-53
        "SFTP or SSH File Transfer Protocol primarily focuses on secure file transfer rather than managing email protocols like both IMAP4 and SMTP.", // A-54
        "SNMP or Simple Network Management Protocol are used to monitor and manage multiple network devices while providing communication between network managers and agents on the network." // A-55
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