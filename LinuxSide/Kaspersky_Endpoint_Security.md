# Kaspersky Endpoint Security

Kasperky's Endpoint Security product, by default, enables real-time protection.

To disable it, run the following command :

```

sudo kesl-control --stop-t 1

```

The "--stop-t" flag stop a specified task number. The documentation indicates that real-time protection runs as task number 1.

To perform a scan we can run this command :

```
sudo kesl-control --scan-file ./filetoscan
```

To view the name of the detected infection run the following command :

```
sudo kesl-control -E --query | grep DetectName
```

The parameter "-E" is to review the event log, and the "--query" to list out the items detected. Then we use "grep" to filter on "DetectName" to display the names of the detected malware.