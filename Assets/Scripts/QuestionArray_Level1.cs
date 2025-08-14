using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestionArray_Level1 : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI answer1Text;
    public TextMeshProUGUI answer2Text;
    public TextMeshProUGUI answer3Text;
    public TextMeshProUGUI explanationText;

    string[] questions = new string[]
    {
        // DL-1
        "All of these are wireless network except for ____",
        // DL-2
        "____ covers a large geographical area i.e., thousands of square kilometers and provide long-distance communication.",
        // DL-3
        "____ is a network that consists of computers,servers, and network devices within an office network.",
        // DL-4
        "All computers are connected through a central hub. Choose the suitable topology for this settings.",
        // DL-5
        "A network security device used to ensure secure communication between two networks.",
        // DL-6
        "The data unit used in data link layer is ____",
        // DL-7
        "The function of ____ layer is to transmit bits over transmission media and it defines the electrical and physical specifications of network devices.",
        // DL-8
        "____ layer encapsulates packets into frames, transmits framees and implement error checking.",
        // DL-9
        "All of these are datalink layer protocol except for ____",
        // DL-10
        "The WiFi and Ethernet networks are standarized by ____",
        // DL-11
        "A ____ address uniquely identifies a network card. Its length is 48 bits. An example of this address is 48-A3-72-1C-8F-4F.",
        // DL-12
        "____ cable is the most common transmission media used by Ethernet. This cable constitutes a combination of insulated copper wires.",
        // DL-13
        "Ethernet frame are in two formats i.e., Ethernet_II and ____",
        // DL-14
        "____ is a process in which a switch forwards the received frame to all interfaces in the switch.",
        // DL-15
        "____ address is used during 'Flooding' phase of a switch.",
        // DL-16
        "____ address is used during 'Forwarding' phase of a switch.",
        // DL-17
        "This address is unique for each network card and assigned based on the manufacturer.",
        // DL-18
        "FF-FF-FF-FF-FF is a ____ address.",
        // DL-19
        "____ protocol is used to bundle multiple physical link in Ethernet into a logical link to increase bandwidth.",
        // DL-20
        "____ is a technology to solve broadcast storm problem by logically divide a large broadcast domain into several broadcast domain.",
        // DL-21
        "In ____, data flows in only one direction.",
        // DL-22
        "In ____, data flows in two direction, but not on the same time.",
        // DL-23
        "In ____, data can simultaneously flow in both direction.",
        // DL-24
        "Walkie-talkie is a device that uses ____ type of communication.",
        // DL-25
        "Ethernet switch is a layer ____ device.",
        // DL-26
        "____ is used on LAN to prevent loops.",
        // DL-27
        "A port in ____ state in STP can send and receive data frames.",
        // DL-28
        "Which one will be elected as a root bridge in STP?",
        // DL-29
        "RSTP is  defined in IEEE ____ as an enhancement to STP.",
        // DL-30
        "A ____ port is located on each non-root bridge and has the shortest cost from the root bridge.",
        // DL-31
        "In STP, after the root bridge is elected, the next step is to identify the ____ port.",
        // DL-32
        "In STP, after the root port is selected, the next step is to identify the ____ port.",
        // DL-33
        "PPP is a datalink layer protocol for ____",
        // DL-34
        "CHAP is an abbreviation for Challenge Handshake ____ Protocol.",
        // DL-35
        "PPP provides the ____ protocol, which is used to negotiate the link layer parameters such as MTU and authentication mode.",
        // DL-36
        "In PPP, the IP address negotiation is done in ____ protocol.",
        // DL-37
        "____ is a link layer protocol that encapsulate PPP frames in Ethernet frames.",
        // DL-38
        "A PPPoE session is established by exchanging different PPPoE packet. ____ is a packet broadcast by a client to request for the service.",
        // DL-39
        "A PPPoE server sends the session ID of the client in ____ packet.",
        // DL-40
        "The checksum of an Ethernet frame is known as Cyclic ____ Check (CRC).",
        // DL-41
        "A network card manufacturer must register with the IEEE to obtain an Organizationally ____ Identifier (OUI).",
        // DL-42
        "A network administrator preconfigures a port VLAN ID (PVID) for each switch port interface. Which type of VLAN assignment for this scenario?",
        // DL-43
        "A port ____ an belong to multiple VLANs, and allows frames from different VLANs to pass through.",
        // DL-44
        "VLAN is a ____ layer protocol.",
        // DL-45
        "STP is a ____ layer protocol.",
        // DL-46
        "In ____ the authenticator creates a Challenge and the peer sends a digest to improve security.",
        // DL-47
        "Datalink layer is in layer ____ in OSI model.",
        // DL-48
        "IEEE 802.11 is a standard for ____ network.",
        // DL-49
        "IEEE 802.3 is a standard for ____ network.",
        // DL-50
        "____ is responsible for the standardization of TCP/IP protocols via the RFC standard series.",
        // DL-51
        "What is the primary function of physical layer in TCP/IP model?",
        // DL-52
        "What is the function of data link control in networking?",
        // DL-53
        "Which device operates at the data link layer?",
        // DL-54
        "What is the purpose of the preamble of Ethernet frame?",
        // DL-55
        "What is the function of the MAC address in networking?"
    };


    string[][] answers = new string[][]
    {
        new string[] { "Bluetooth", "WiFi", "Ethernet" }, // DL-1
        new string[] { "LAN", "MAN", "WAN" }, // DL-2
        new string[] { "WAN", "PAN", "LAN" }, // DL-3
        new string[] { "Ring", "Full-mesh", "Star" }, // DL-4
        new string[] { "Access Point", "Switch", "Firewall" }, // DL-5
        new string[] { "packet", "datagram", "frame" }, // DL-6
        new string[] { "Transport", "Application", "Physical"}, // DL-7
        new string[] { "Network", "Application", "Datalink" }, // DL-8
        new string[] { "Ethernet", "PPP", "HTTP" }, // DL-9
        new string[] { "IETF", "ISO", "IEEE" }, // DL-10
        new string[] { "Port", "IP", "MAC"}, // DL-11
        new string[] { "Optical fiber", "Coaxial", "Twisted pair"}, // DL-12
        new string[] { "IEEE 802.4", "IEEE 802.11", "IEEE 802.3"}, // DL-13
        new string[] { "Forwarding", "Discarding", "Flooding"}, // DL-14
        new string[] { "Multicast", "Unicast", "Broadcast"}, // DL-15
        new string[] { "Multicast", "Anycast", "Unicast"}, // DL-16
        new string[] { "Port number", "IP address", "MAC address"}, // DL-17
        new string[] { "Multicast", "Unicast", "Broadcast"}, // DL-18
        new string[] { "WiFi", "PPP", "LACP"}, // DL-19
        new string[] { "WLAN", "VPN", "VLAN"}, // DL-20
        new string[] { "Half-Duplex", "Full-Duplex", "Simplex"}, // DL-21
        new string[] { "Full-Duplex", "Simplex", "Half-Duplex"}, // DL-22
        new string[] { "Simplex", "Half-Duplex", "Full-Duplex"}, // DL-23
        new string[] { "Half-Duplex", "Full-Duplex", "Simplex"}, // DL-24
        new string[] { "5", "3", "2"}, // DL-25
        new string[] { "TCP", "VLAN", "STP"}, // DL-26
        new string[] { "Blocking", "Listening", "Forwarding"}, // DL-27
        new string[] { "Smallest PID", "Smallest RPC", "Smallest BID" }, // DL-28
        new string[] { "802.1y", "802.1x", "802.1w" }, // DL-29
        new string[] { "alternate", "designated", "root" }, // DL-30
        new string[] { "edge", "alternate", "root" }, // DL-31
        new string[] { "edge", "backup", "designated" }, // DL-32
        new string[] { "PAN", "MAN", "WAN" }, // DL-33
        new string[] { "Authorization", "Accounting", "Authentication" }, // DL-34
        new string[] { "PAP", "CHAP", "LCP" }, // DL-35
        new string[] { "CHAP", "LCP", "IPCP" }, // DL-36
        new string[] { "PPPoC", "PPPoA", "PPPoE" }, // DL-37
        new string[] { "PADR", "PADO", "PADI" }, // DL-38
        new string[] { "PADI", "PADR", "PADS" }, // DL-39
        new string[] { "Ready", "Recovery", "Redundancy" }, // DL-40
        new string[] { "Urgent", "Universal", "Unique" }, // DL-41
        new string[] { "MAC address-based VLAN", "IP subnet-based VLAN", "Interface-based VLAN" }, // DL-42
        new string[] { "Edge", "Access", "Trunk" }, // DL-43
        new string[] { "Network", "Transport", "Datalink" }, // DL-44
        new string[] { "Transport", "Application", "Datalink" }, // DL-45
        new string[] { "NCP", "PAP", "CHAP" }, // DL-46
        new string[] { "4", "3", "2" }, // DL-47
        new string[] { "STP", "VLAN", "WiFi" }, // DL-48
        new string[] { "VLAN", "WiFi", "Ethernet" }, // DL-49
        new string[] { "EIA", "ISO", "IETF" }, // DL-50
        new string[] { "Data segmentation and reassembly", "Error correction", "Signal encoding and transmission" }, // DL-51
        new string[] { "Multiplexing", "Segmentation", "Flow and error control" }, // DL-52
        new string[] { "Hub", "Router", "Switch" }, // DL-53
        new string[] { "Providing error correction", "Encrypting data", "Synchronizing sender and receiver" }, // DL-54
        new string[] { "Providing data encryption", "Establishing physical connections", "Identifying devices on a network" } // DL-55
    };

    string[] explanations = new string[]
    {
        "Ethernet is not a wireless network; it's a wired technology used for local area networks (LANs) to connect devices via cables.", // DL-1
        "WANs cover extensive geographical areas, facilitating long-distance communication across cities, countries, or even globally.", // DL-2
        "LANs comprise computers, servers, and network devices interconnected within a limited geographic area like an office, enabling local data sharing and resource access.", // DL-3
        "In a star topology, all computers connect to a central hub or switch individually, forming a centralized network architecture that allows each device to communicate directly with the central hub.", // DL-4
        "Firewalls ensure secure communication between networks by filtering incoming and outgoing network traffic, protecting against unauthorized access and threats.", // DL-5
        "Frames are the data units used in the Data Link layer, encapsulating packets for transmission across the network.", // DL-6
        "The Physical layer in the OSI model deals with the transmission of raw data bits over the network medium, defining the electrical and physical characteristics of the devices.", // DL-7
        "The Data Link layer encapsulates packets into frames, manages frame transmission, and implements error checking mechanisms.", // DL-8
        "HTTP operates at the Application layer (Layer 7) of the OSI model and is not a Data Link layer protocol.", // DL-9
        "The IEEE (Institute of Electrical and Electronics Engineers) standardizes protocols for various technologies, including WiFi and Ethernet networks.", // DL-10
        "MAC (Media Access Control) address uniquely identifies network cards, comprising 48 bits and assigned by the manufacturer.", // DL-11
        "Twisted pair cables are commonly used by Ethernet for data transmission, consisting of insulated copper wires.", // DL-12
        "Ethernet frames adhere to two formats: Ethernet II and IEEE 802.3, each defining different frame structures and fields.", // DL-13
        "Flooding is a process wherein a switch forwards received frames to all interfaces except the incoming interface.", // DL-14
        "Broadcast addresses are used during the 'Flooding' phase of a switch to ensure frames reach all devices in the network.", // DL-15
        "Unicast addresses are used during the 'Forwarding' phase of a switch to send frames to a specific device.", // DL-16
        "MAC addresses are unique for each network card and are assigned by the manufacturer.", // DL-17
        "The address 'FF-FF-FF-FF-FF' represents a Broadcast address in Ethernet networks.", // DL-18
        "LACP (Link Aggregation Control Protocol) bundles multiple physical links in Ethernet into a logical link to enhance bandwidth.", // DL-19
        "VLANs (Virtual Local Area Networks) logically segment a large broadcast domain into smaller domains to improve network performance and security.", // DL-20
        "Simplex communication allows data to flow in only one direction, like a walkie-talkie.", // DL-21
        "Half-Duplex communication permits data to flow in two directions but not simultaneously, akin to a two-way radio.", // DL-22
        "Full-Duplex communication enables simultaneous data flow in both directions, like a phone conversation.", // DL-23
        "Walkie-talkies employ Simplex communication, allowing transmission in one direction at a time.", // DL-24
        "Ethernet switches function at the Data Link layer (Layer 2) of the OSI model.", // DL-25
        "STP (Spanning Tree Protocol) prevents loops in LANs by blocking redundant paths and selecting a loop-free topology.", // DL-26
        "A port in the 'Forwarding' state in STP forwards data frames and receives frames from connected devices.", // DL-27
        "The root bridge in STP is elected based on the smallest Bridge ID (BID), ensuring a central point in the network topology.", // DL-28
        "RSTP (Rapid Spanning Tree Protocol) is an IEEE 802.1w standard, an enhancement to the original STP, offering faster convergence.", // DL-29
        "The root port, found on each non-root bridge, has the shortest path cost to the root bridge in STP.", // DL-30
        "After electing the root bridge in STP, the next step is to identify the 'Root Port' on each non-root bridge.", // DL-31
        "After selecting the root port in STP, the next step is to identify the 'Designated Port' on each network segment.", // DL-32
        "PPP (Point-to-Point Protocol) is a Data Link layer protocol used for WAN (Wide Area Network) communication.", // DL-33
        "CHAP (Challenge Handshake Authentication Protocol) provides a secure method for authenticating PPP connections.", // DL-34
        "PPP's Link Control Protocol (LCP) negotiates link layer parameters such as MTU and authentication mode.", // DL-35
        "In PPP, the IP Control Protocol (IPCP) negotiates IP address configuration for the PPP connection.", // DL-36
        "PPPoE (Point-to-Point Protocol over Ethernet) encapsulates PPP frames within Ethernet frames for transmission.", // DL-37
        "PPPoE uses a 'PADI' (PPP Active Discovery Initiation) packet, broadcast by a client to discover a PPPoE server.", // DL-38
        "A PPPoE server sends the 'PADS' (PPP Active Discovery Session-confirmation) packet containing the session ID of the client.", // DL-39
        "The checksum of an Ethernet frame, known as Cyclic Redundancy Check (CRC), helps detect errors in transmitted data.", // DL-40
        "Network card manufacturers obtain an Organizationally Unique Identifier (OUI) from the IEEE, ensuring unique MAC address assignment.", // DL-41
        "Interface-based VLAN assignment configures a Port VLAN ID (PVID) for each switch port interface in a network.", // DL-42
        "A Trunk port on a network device can belong to multiple VLANs, enabling frames from different VLANs to pass through.", // DL-43
        "VLANs operate at the Data Link layer, facilitating logical segmentation of networks.", // DL-44
        "STP operates at the Data Link layer, providing loop prevention and redundancy elimination in network topologies.", // DL-45
        "CHAP (Challenge Handshake Authentication Protocol) involves the authenticator challenging the peer for enhanced security in authentication.", // DL-46
        "The Data Link layer is the second layer (Layer 2) in the OSI model, responsible for framing and addressing.", // DL-47
        "IEEE 802.11 standards define WiFi networks, enabling wireless communication and access.", // DL-48
        "IEEE 802.3 standards define Ethernet networks, specifying the wired communication protocol.", // DL-49
        "The IETF (Internet Engineering Task Force) standardizes TCP/IP protocols through the RFC series, ensuring internet protocol standardization.", // DL-50
        "Physical layer in TCP/IP model is responsible for both encode and transmit signals while also facilitating the transmission of data across the physical medium.", // DL-51
        "Data link layer in TCP/IP model implement flow and error control to manage the flow of data transmission ensure data delivery accuracy over the network.", // DL-52
        "Switch operate at the data link layer to provide effective data forwarding based on MAC addresses within a local network area (known as LAN).", // DL-53
        "The preamble of an Ethernet frame will synchronizes the clocks of both sender and receiver to ensure accurate timing for the data transmission.", // DL-54
        "MAC addresses are used to uniquely identify any device within a network which provides effective communication and data transfer between them." // DL-55
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