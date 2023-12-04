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
        title: title,
        message: message || "",
    };
    if (type === "success") {
    }

    notifications.show({
        color: "green",
        title: "Question created successfully",
        message: "",
        classNames: styles,
    });
}
