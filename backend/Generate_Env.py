import psutil
import socket
 
def list_interfaces():
    # Lista todas as interfaces para identificar o nome correto do Wi-Fi
    interfaces = psutil.net_if_addrs()
    print("Interfaces disponíveis:")
    for interface, addrs in interfaces.items():
        print(f"{interface}: {[addr.address for addr in addrs if addr.family == socket.AF_INET]}")
    return interfaces
 
def get_wifi_ip_address():
    interfaces = list_interfaces()
    for interface, addrs in interfaces.items():
        # Altere "Wi-Fi" para o nome que corresponde à sua interface Wi-Fi
        if "Wi-Fi" in interface or "wifi" in interface.lower():
            for addr in addrs:
                if addr.family == socket.AF_INET:  # Verifica se é IPv4
                    return addr.address
    return None  # Retorna None se o adaptador Wi-Fi não tiver IPv4
 
def write_env_file(ip_address):
    with open('.env', 'w') as f:
        f.write(f"DB_HOST={ip_address}\n")
 
if __name__ == "__main__":
    ip_address = get_wifi_ip_address()
    if ip_address:
        write_env_file(ip_address)
        print(f"Endereço IP do Wi-Fi ({ip_address}) escrito no arquivo .env")
    else:
        print("Não foi possível encontrar um endereço IPv4 válido.")