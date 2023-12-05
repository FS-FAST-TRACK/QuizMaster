import { FunnelIcon } from "@heroicons/react/24/outline";
import { Popover } from "@mantine/core";

export default function QuestionFilter() {
    return (
        <div>
            <Popover width={200} withArrow position="bottom">
                <Popover.Target>
                    <div className="cursor-pointer w-10 flex items-center justify-center aspect-square">
                        <FunnelIcon className="w-6" />
                    </div>
                </Popover.Target>
                <Popover.Dropdown>
                    <div className="flex">
                        <div>Category</div>
                        <div>=</div>
                    </div>
                </Popover.Dropdown>
            </Popover>
        </div>
    );
}
