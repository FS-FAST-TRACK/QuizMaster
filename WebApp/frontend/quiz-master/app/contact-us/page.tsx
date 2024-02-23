import contactUs from "/public/amico.svg";
import Image from "next/image";
import ContactDetails from "@/components/Commons/modals/contactDetails";
import ContactUsForm from "@/components/Commons/form/ContactUsForm";
import { getServerSession } from "next-auth";

export default async function ContactUs() {
    const session = await getServerSession();
    return (
        <div className="flex md:flex-row flex-col w-full gap-5 bg-white p-8 rounded-xl shadow-2xl">
            <div className="flex flex-col gap-2 flex-1">
                <p className=" font-bold text-3xl">Contact Us</p>
                <p className="text-sm pt-3">
                    If you have any inquiries, get in touch with us. Just fill
                    in the necessary fields on the form and we would be happy to
                    hear your thoughts out.
                </p>
                <div>
                    <Image
                        src={contactUs}
                        alt="Contact Us"
                        width={500}
                        height={500}
                    />
                </div>
                <div>
                    <ContactDetails email={session?.user.email} />
                </div>
            </div>
            <ContactUsForm />
        </div>
    );
}
