import React from "react";
import DashboardPage from "../../../pages/DashboardPage/DashboardPage";
import Menu from "../../../pages/Menu/Menu";

function DashboardESD() {
  return <Menu componentToShow={<DashboardPage />} />;
}

export default DashboardESD;
