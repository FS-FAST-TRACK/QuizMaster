import React from "react";
import { Page, Text, Document, StyleSheet, View } from "@react-pdf/renderer";
import { User } from "../usersReport";
import moment from "moment";

export function UsersTablePDF({ users }: { users: User[] }) {
    return (
        <View>
            <View style={styles.table}>
                <View style={styles.row}>
                    <Text style={[styles.column1, styles.tableHeader]}>
                        Last Name
                    </Text>
                    <Text style={[styles.column2, styles.tableHeader]}>
                        First Name
                    </Text>
                    <Text style={[styles.column3, styles.tableHeader]}>
                        User Name
                    </Text>
                    <Text style={[styles.column4, styles.tableHeader]}>
                        Email
                    </Text>
                    <Text style={[styles.column5, styles.tableHeader]}>
                        Active
                    </Text>
                    <Text style={[styles.column6, styles.tableHeader]}>
                        Roles
                    </Text>
                </View>
                {users.map((user, index) => {
                    return <UserTableRow user={user} key={index} />;
                })}
            </View>
        </View>
    );
}

function UserTableRow({ user }: { user: User }) {
    return (
        <View style={styles.row}>
            <Text style={styles.column1}>{user.lastName}</Text>
            <Text style={styles.column2}>{user.firstName}</Text>
            <Text style={styles.column3}>{user.userName}</Text>
            <Text style={styles.column4}>{user.email}</Text>
            <Text style={[styles.column5]}>
                {user.activeData ? "Active" : "Inactive"}
            </Text>
            <Text style={styles.column6}>{user.roles.join(" ")}</Text>
        </View>
    );
}

const styles = StyleSheet.create({
    row: {
        flexDirection: "row",
        alignItems: "center",
    },
    tableHeader: {
        borderTopWidth: 0,
        fontWeight: 700,
        paddingVertical: 8,
        backgroundColor: "#17a14b",
        color: "white",
    },
    table: {
        borderWidth: 1,
        borderTopWidth: 1,
        borderTopColor: "#17a14b",
        borderColor: "#D1D5DB",
        borderRadius: 6,
        overflow: "hidden",
        marginBottom: 32,
    },
    sectionTitle: {
        fontSize: 12,
        marginBottom: 8,
        fontWeight: 600,
    },
    column1: {
        width: "17%",
        height: "100%",
        textAlign: "center",
        fontSize: 8,
        paddingVertical: 8,
        paddingHorizontal: 8,
        borderTopWidth: 1,
        borderColor: "#D1D5DB",
    },
    column2: {
        width: "17%",
        height: "100%",
        fontSize: 8,
        paddingVertical: 8,
        paddingHorizontal: 8,
        borderTopWidth: 1,
        borderLeftWidth: 1,
        borderColor: "#D1D5DB",
    },

    column3: {
        width: "17%",
        height: "100%",
        fontSize: 8,
        paddingVertical: 8,
        paddingHorizontal: 8,
        borderTopWidth: 1,
        borderLeftWidth: 1,
        borderColor: "#D1D5DB",
    },
    column4: {
        width: "17%",
        height: "100%",
        fontSize: 8,
        paddingVertical: 8,
        paddingHorizontal: 8,
        borderTopWidth: 1,
        borderLeftWidth: 1,
        borderColor: "#D1D5DB",
    },
    column5: {
        width: "17%",
        height: "100%",
        fontSize: 8,
        paddingVertical: 8,
        paddingHorizontal: 8,
        borderTopWidth: 1,
        borderLeftWidth: 1,
        borderColor: "#D1D5DB",
    },
    column6: {
        width: "17%",
        height: "100%",
        fontSize: 8,
        paddingVertical: 8,
        paddingHorizontal: 8,
        borderTopWidth: 1,
        borderLeftWidth: 1,
        borderRightWidth: 1,
        borderColor: "#D1D5DB",
    },
});
