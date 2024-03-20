// AuthProvider.js

import React, { createContext, useState, useEffect } from 'react';

const AuthContext = createContext();

const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);

  // Kullanıcı token verisini kontrol et
  useEffect(() => {
    const token = localStorage.getItem('token');
    if (token) {
      // Token varsa decode et ve expire olmuş mu bak
      const decodedUser = decodeToken(token);
      if (decodedUser && !isTokenExpired(decodedUser.exp)) {
        setUser(decodedUser);
      } else {
        // Token geçerli değil ise çıkış yap
        logout();
      }
    }
  }, []);

  // Login işlemi
  const login = (token) => {
    const decodedUser = decodeToken(token);
    setUser(decodedUser);
    // Tokeni kaydet
    localStorage.setItem('token', token);
  };

  // Çıkış işlemi
  const logout = () => {
    setUser(null);
    localStorage.removeItem('token');
  };

  // Token decode
  const decodeToken = (token) => {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload;
    } catch (error) {
      return null;
    }
  };

  const isTokenExpired = (exp) => {
    return exp < Date.now() / 1000;
  };

  return (
    <AuthContext.Provider value={{ user, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export { AuthProvider, AuthContext };
