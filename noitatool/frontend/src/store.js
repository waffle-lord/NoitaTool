import { writable } from "svelte/store";

export const notifications = writable([]);

export const addNotification = (notification) => {
    const id = Math.floor(Math.random() * 10000)

    const defaults = {
        id,
        type: "info",
        message: ":O"
    };

    notifications.update((all) => [{ ...defaults, ...notification }, ...all]);

    return id;
};

export const updateNotification = (id, notification, closing = false) => {
    notification.id = id;
    notifications.update(all => {
        const i = all.findIndex((n) => n.id == id);
        all[i] = notification
        return all
    });

    if (closing) {
        setTimeout(function() {dismissNotification(id)}, 3000);
    }
};

export const dismissNotification = (id) => {
    notifications.update((all) => all.filter((n) => n.id !== id));
}