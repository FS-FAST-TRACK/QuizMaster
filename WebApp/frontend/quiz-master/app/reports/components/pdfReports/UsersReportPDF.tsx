import React from "react";
import { Page, Text, Document, StyleSheet, View } from "@react-pdf/renderer";
import { User } from "../usersReport";
import { UsersTablePDF } from "./UsersTablePDF";

export function UsersReportPDF({ users }: { users: User[] }) {
    return (
        <Document>
            <Page style={styles.body}>
                <View style={styles.header}>
                    <Text style={styles.sectionTitle}>
                        Quiz Master User Reports
                    </Text>
                    <Text style={styles.count}>{users.length} users</Text>
                </View>
                <UsersTablePDF users={users} />
            </Page>
        </Document>
    );
}

const styles = StyleSheet.create({
    body: {
        paddingTop: 32,
        paddingBottom: 32,
        paddingHorizontal: 32,
    },
    header: {
        flexDirection: "row",
        justifyContent: "space-between",
    },
    count: {
        color: "grey",
        fontSize: 12,
    },
    sectionTitle: {
        fontSize: 14,
        marginBottom: 8,
        fontWeight: 600,
    },
});
