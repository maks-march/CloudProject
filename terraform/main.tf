resource "yandex_vpc_network" "default" {
  name = var.network
}

resource "yandex_vpc_subnet" "default" {
  network_id     = yandex_vpc_network.default.id
  name           = var.subnet
  v4_cidr_blocks = var.subnet_v4_cidr_blocks
  zone           = var.zone
}

resource "yandex_vpc_security_group" "vm-sg" {
  name       = "vm-security-group"
  network_id = yandex_vpc_network.default.id

  ingress {
    protocol       = "TCP"
    port           = 80
    v4_cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    protocol       = "TCP"
    port           = 443
    v4_cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    protocol       = "TCP"
    port           = 22
    v4_cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    protocol       = "ANY"
    v4_cidr_blocks = ["0.0.0.0/0"]
  }
}

data "yandex_compute_image" "default" {
  family = var.image_family
}

resource "yandex_compute_instance" "default" {
  name     = var.name
  hostname = var.name
  zone     = var.zone

  resources {
    cores  = var.cores
    memory = var.memory
  }

  boot_disk {
    initialize_params {
      image_id = data.yandex_compute_image.default.id
      size     = var.disk_size
      type     = var.disk_type
    }
  }

  network_interface {
    subnet_id          = yandex_vpc_subnet.default.id
    nat                = var.nat
    security_group_ids = [yandex_vpc_security_group.vm-sg.id]
  }

  metadata = {
    user-data = <<-EOF
      #cloud-config
      users:
        - name: ${var.user_name}
          passwd: ${var.user_pass}
          sudo: ALL=(ALL) NOPASSWD:ALL
          shell: /bin/bash
          ssh_authorized_keys:
            - ${var.ssh_public_key}
      write_files:
        - path: /app/appsettings.json
          content: |
            {
              "Logging": {
                "LogLevel": {
                  "Default": "Information",
                  "Microsoft.AspNetCore": "Warning"
                }
              },
              "Kestrel": {
                "Endpoints": {
                  "Http": {
                    "Url": "http://0.0.0.0:5089"
                  }
                }
              },
              "AllowedHosts": "*",
              "ConnectionStrings": {
                "DefaultConnection": "Host=${yandex_mdb_postgresql_cluster.postgres.host[0].fqdn};Port=6432;Database=MainDB;Username=${var.db_user};Password=${var.db_password};SslMode=Require;"
              }
            }
      runcmd:
        - apt-get update
        - apt-get install -y docker.io docker-compose
        - systemctl enable docker
        - systemctl start docker
        - rm -rf /app
        - git clone https://github.com/maks-march/CloudProject /app
        - cd /app && docker-compose up -d
      EOF
  }

  timeouts {
    create = var.timeout_create
    delete = var.timeout_delete
  }
}

resource "yandex_mdb_postgresql_cluster" "postgres" {
  name        = "marketplace-db"
  environment = "PRODUCTION"
  network_id  = yandex_vpc_network.default.id

  config {
    version = 15
    resources {
      resource_preset_id = "s2.micro"
      disk_type_id       = "network-ssd"
      disk_size          = 10
    }
  }

  host {
    zone      = "ru-central1-a"
    subnet_id = yandex_vpc_subnet.default.id
  }

  user {
    name     = var.db_user
    password = var.db_password
  }

  database {
    name  = "MainDB"
    owner = var.db_user
  }
}

output "name" {
  value = yandex_compute_instance.default.name
}

output "address" {
  value = yandex_compute_instance.default.network_interface.0.nat_ip_address
}

output "postgres_host" {
  value = yandex_mdb_postgresql_cluster.postgres.host[0].fqdn
}

