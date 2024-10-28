import React, { useContext, useEffect, useState } from "react";
import { ContentContext } from "../../context/appContex";
import "../../css/notification.css";

const Notification = () => {
  const { notification, clearNotification } = useContext(ContentContext);
  const [isVisible, setIsVisible] = useState(false);

  useEffect(() => {
    if (notification.message) {
      setIsVisible(true); // Muestra la notificación

      const timer = setTimeout(() => {
        setIsVisible(false); // Oculta la notificación después de 3 segundos
        clearNotification();
      }, 3000);

      return () => clearTimeout(timer);
    }
  }, [notification, clearNotification]);

  if (!notification.message) return null;

  return (
    <div id="liveAlertPlaceholder">
      <div
        className={`notification ${notification.type} ${
          isVisible ? "show" : "hide"
        }`}
        onClick={clearNotification}
        role="alert"
      >
        {notification.message}
      </div>
    </div>
  );
};

export default Notification;
