import React, { useState, useEffect } from "react";
import { Layout, Menu, Typography, Button, Card, Spin, Dropdown } from "antd";
import {
  UserOutlined,
  ApartmentOutlined,
  LogoutOutlined,
  DownOutlined,
} from "@ant-design/icons";
import SearchOutlinedIcon from "@mui/icons-material/SearchOutlined";
import NotificationsNoneOutlinedIcon from "@mui/icons-material/NotificationsNoneOutlined";
import HelpOutlineOutlinedIcon from "@mui/icons-material/HelpOutlineOutlined";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import TranslateOutlinedIcon from "@mui/icons-material/TranslateOutlined";
import Logo from "./logo-compal.png";
import "./Menu.css";
import { MenuFoldOutlined, MenuUnfoldOutlined } from "@ant-design/icons";
const { Header, Sider, Content } = Layout;

// Definição dos tipos para os dados do menu
interface MenuItem {
  text: string;
  icon: React.ReactNode;
  path: string;
  roles: string[];
  subItems?: { text: string; path: string }[];
}

// Funções de utilitário
const getUserRoleFromToken = (token: string | null): string => {
  return token === "administrator" ? "administrador" : "operator";
};

const getMenuItems = (userRole: string): MenuItem[] => {
  const allItems: MenuItem[] = [
    {
      text: "Controle",
      icon: <ApartmentOutlined />,
      path: "/dashboard",
      roles: ["administrator", "operator"],
      subItems: [
        { text: "Dashboard", path: "/dashboard" }
      ],
    }
  ];

  return allItems.filter((item) => item.roles.includes(userRole));
};

// Componente do MenuList
interface MenuListProps {
  menuItems: MenuItem[];
}

const MenuList: React.FC<MenuListProps> = ({ menuItems }) => {
  const location = useLocation();
  const currentPath = location.pathname;

  return (
    <Menu mode="inline" theme="dark" defaultSelectedKeys={[currentPath]}>
      {menuItems.map((item, index) => (
        <Menu.SubMenu
          key={item.path}
          title={
            <>
              {item.icon}
              <span>{item.text}</span>
            </>
          }
        >
          {item.subItems &&
            item.subItems.map((subItem, subIndex) => (
              <Menu.Item key={subItem.path}>
                <Link to={subItem.path}>{subItem.text}</Link>
              </Menu.Item>
            ))}
        </Menu.SubMenu>
      ))}
    </Menu>
  );
};

// Componente principal Menu
interface MenuProps {
  componentToShow: React.ReactNode;
}

const MenuComponent: React.FC<MenuProps> = ({ componentToShow }) => {
  const navigate = useNavigate();

  const token = localStorage.getItem("role");
  const name = localStorage.getItem("name");
  const userRole = getUserRoleFromToken(token);
  const menuItems = getMenuItems(userRole);
  const { logout } = useAuth();
  const [isLoading, setIsLoading] = useState(true);
  const [collapsed, setCollapsed] = useState(false); // Estado para colapsar o Sider

  const onLogout = () => {
    localStorage.removeItem("token");
    navigate("/");
  };

  useEffect(() => {
    const timer = setTimeout(() => {
      setIsLoading(false);
    }, 1000);

    return () => clearTimeout(timer);
  }, []);
  const userMenu = (
    <Menu>
      <Menu.Item key="logout" onClick={onLogout}>
        <LogoutOutlined /> Sair
      </Menu.Item>
    </Menu>
  );

  return (
    <Layout>
      <Sider collapsible collapsed={collapsed}>
        <div className="collapse-button-container">
          <Button
            type="text"
            icon={collapsed ? <MenuUnfoldOutlined /> : <MenuFoldOutlined />}
            onClick={() => setCollapsed(!collapsed)}
            className="collapse-button"
          />
        </div>
        <MenuList menuItems={menuItems} />
      </Sider>
      <Layout>
        <Header>
          <div className="header">
            <Typography.Title level={3}>
              <img src={Logo} alt="" width="200px" className="logo-header" />
            </Typography.Title>
            <div>
              <SearchOutlinedIcon
                style={{ marginRight: 30, color: "#FFFFFF" }}
              />
              <HelpOutlineOutlinedIcon
                style={{ marginRight: 30, color: "#FFFFFF" }}
              />

              <NotificationsNoneOutlinedIcon
                style={{ marginRight: 30, color: "#FFFFFF" }}
              />
              {/* <Typography.Text style={{ marginRight: 30, color: "#FFFFFF" }}>
                {name}
              </Typography.Text> */}
              <TranslateOutlinedIcon
                style={{ marginRight: 10, color: "#FFFFFF" }}
              />
              <Dropdown overlay={userMenu} trigger={["click"]}>
                <Typography.Text
                  style={{
                    marginRight: 3,
                    color: "#FFFFFF",
                    cursor: "pointer",
                  }}
                >
                  {name}
                  <DownOutlined />
                </Typography.Text>
              </Dropdown>

              {/* <Button
                icon={<LogoutOutlined />}
                type="primary"
                onClick={logout}
                href="/login"
              >
                Sair
              </Button> */}
            </div>
          </div>
        </Header>
        <Content style={{ margin: "20px" }}>
          <Card className="main-card" style={{ overflow: "auto", minHeight: "500px" }}>
            {isLoading ? (
              <div className="loading-spinner">
                <Spin size="large" />
              </div>
            ) : (
              componentToShow
            )}
          </Card>
          
        <footer className="app-footer">
          <p>&copy; 2024 Compal. Todos os direitos reservados.</p>
        </footer>
        </Content>
      </Layout>
    </Layout>
  );
};

export default MenuComponent;
