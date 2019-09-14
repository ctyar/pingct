# pingct

[![Build Status](https://dev.azure.com/ctyar/pingct/_apis/build/status/ctyar.pingct?branchName=master)](https://dev.azure.com/ctyar/pingct/_build/latest?definitionId=3&branchName=master)

A simple Ping like tool to check the common network connection issues in Iran.

As network issues are very common and happen for different reasons `pingct` is designed to check network connectivity to a remote IP address and do a certain set of tests in case of connection issue to facilitate troubleshooting.

Currently, the tests are:
1. Pinging the gateway IP address
1. Pinging `aparat.com` to see if the internal network is accessible
1. Testing DNS
1. Trying to access a restricted IP to see if our restriction bypass solution is working

Output sample:
```
> pingct 
Reply from 4.2.2.4: time=111ms                                                                                                                                                               
Reply from 4.2.2.4: time=106ms                                                                                                                                                                
Reply from 4.2.2.4: time=105ms                                                                                                                                                                
Reply from 4.2.2.4: time=107ms                                                                                                                                                                
Reply from 4.2.2.4: time=105ms                                                                                                                                                                
Reply from 4.2.2.4: time=112ms 
    Reply from 192.168.1.1: time=2ms
    Reply from aparat.com: time=0ms
    DNS: OK
    Freedom: Not working
Reply from 4.2.2.4: time=148ms
Reply from 4.2.2.4: time=152ms
Reply from 4.2.2.4: time=154ms
Reply from 4.2.2.4: time=159ms
Reply from 4.2.2.4: time=154ms
Reply from 4.2.2.4: time=153ms
```
