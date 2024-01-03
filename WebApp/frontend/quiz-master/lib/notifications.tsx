import { notifications } from "@mantine/notifications";
import styles from "@/styles/notification.module.css";

export function notification({
    type,
    title,
    message,
}: {
    type: "success" | "error" | "info";
    title: string;
    message?: string;
}) {
    var notifData = {
        color: "green",
        title: title,
        message: message || "",
    };

    switch (type) {
        case "success":
            notifData.color = "green";
            break;
        case "error":
            notifData.color = "red";
            break;
        case "info":
            notifData.color = "blue";
            break;
    }

    notifications.show({
        ...notifData,
        classNames: styles,
    });
}
