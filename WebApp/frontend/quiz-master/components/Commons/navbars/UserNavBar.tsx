interface UserNavBarProps {
    userName: string;
    email: string;
}
export default function UserNavBar({ userName, email }: UserNavBarProps) {
    return (
        <div className="transition-all duration-300 items-center py-3 border-t  text-sm font-medium hover:bg-[--primary-200] px-3">
            <div className="text-sm font-medium">{userName}</div>
            <div className="text-xs text-gray-500 ">{email}</div>
        </div>
    );
}
