import { useState, useEffect } from "react";
import { jwtDecode } from "jwt-decode";

export default function useUserTokenData() {
  const [userData, setUserData] = useState(null);
  const [isAdmin, setIsAdmin] = useState(false);

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (token) {
      const data = jwtDecode(token);
      setUserData(JSON.parse(data.token));
      const user = JSON.parse(data.token);
      setIsAdmin(user["Roles"].includes("Administrator"));
    }
  }, []);

  return {
    userData,
    isAdmin,
  };
}
