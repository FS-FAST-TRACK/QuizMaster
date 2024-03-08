import { EllipsisHorizontalIcon } from "@heroicons/react/24/outline";
import LoadingTable from "./loadingTable";
import { User } from "./usersReport";
import { Badge, Table } from "@mantine/core";
import moment from "moment";

const TABLE_COLUMNS = [
    "Last Name",
    "First Name",
    "User Name",
    "Email",
    "Active",
    "Role",
    "Account Created",
    "",
];

export function UsersTable({
    users,
    isLoading,
}: {
    users: User[];
    isLoading: boolean;
}) {
    if (isLoading) {
        return <LoadingTable columnNames={TABLE_COLUMNS} numberOfRows={30} />;
    }

    return (
        <div className="border border-gray-300 rounded-md">
            <Table>
                <Table.Thead>
                    <Table.Tr>
                        {TABLE_COLUMNS.map((column, index) => {
                            return <Table.Td key={index}>{column}</Table.Td>;
                        })}
                    </Table.Tr>
                </Table.Thead>
                <Table.Tbody>
                    {users.map((user, index) => {
                        return <UserTableRow user={user} key={index} />;
                    })}
                </Table.Tbody>
            </Table>
        </div>
    );
}

function UserTableRow({ user }: { user: User }) {
    return (
        <Table.Tr>
            <Table.Td>
                {user.lastName || (
                    <p className="text-gray-400 text-xs">None provided</p>
                )}
            </Table.Td>
            <Table.Td>
                {user.firstName || (
                    <p className="text-gray-400 text-xs">None provided</p>
                )}
            </Table.Td>
            <Table.Td>
                {user.userName || (
                    <p className="text-gray-400 text-xs">None provided</p>
                )}
            </Table.Td>
            <Table.Td>
                {user.email || (
                    <p className="text-gray-400 text-xs">None provided</p>
                )}
            </Table.Td>
            <Table.Td>
                {user.activeData ? (
                    <Badge color="green">Active</Badge>
                ) : (
                    <Badge color="gray">Inactive</Badge>
                )}
            </Table.Td>
            <Table.Td>{user.roles}</Table.Td>
            <Table.Td>
                {moment(user.dateCreated).isValid()
                    ? moment(user.dateCreated).format("ll")
                    : ""}
            </Table.Td>
        </Table.Tr>
    );
}
