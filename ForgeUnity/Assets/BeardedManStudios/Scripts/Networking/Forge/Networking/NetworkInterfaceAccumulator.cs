﻿using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace BeardedManStudios.Forge.Networking
{
	public class NetworkInterfaceAccumulator
	{
		public List<IPAddress> IPs { get; private set; }

		public NetworkInterfaceAccumulator()
		{
			IPs = new List<IPAddress>();
		}

		public void Accumulate(AddressFamily family)
		{
			IPs.Clear();
			foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
			{
				AddNetworkInterfaceIfPossible(nic, family);
			}
		}

		private void AddIfInFamily(NetworkInterface nic, AddressFamily family)
		{
			foreach (UnicastIPAddressInformation ip in nic.GetIPProperties().UnicastAddresses)
			{
				if (ip.Address.AddressFamily == family)
				{
					IPs.Add(ip.Address);
				}
			}
		}

		private void AddNetworkInterfaceIfPossible(NetworkInterface nic, AddressFamily family)
		{
			switch (nic.NetworkInterfaceType)
			{
				case NetworkInterfaceType.Wireless80211:
				case NetworkInterfaceType.Ethernet:
					break;
				default:
					return;
			}

			if (nic.OperationalStatus != OperationalStatus.Up)
				return;

			AddIfInFamily(nic, family);
		}
	}
}
