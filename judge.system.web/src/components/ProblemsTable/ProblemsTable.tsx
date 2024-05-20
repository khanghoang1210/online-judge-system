import Link from 'next/link';
import React, { useEffect } from 'react';
import { useCookies } from 'react-cookie';
import { BsCheckCircle } from 'react-icons/bs';

type Problem = {
    id: string;
    title: string;
    difficulty: string;
    tagId: string
    //category: string;
    // Add more properties if needed
};

type ProblemsTableProps = {};

const API_URL = 'https://localhost:7004/api/Problems/';

const ProblemsTable: React.FC<ProblemsTableProps> = () => {
    const [problemList, setProblemList] = React.useState<Problem[]>([]);
    const [token] = useCookies(['token']);

    useEffect(() => {
        fetch(API_URL, {
            method: 'GET',
            mode: "cors",
            headers: {
                'Accept': 'application/json, text/plain',
                'Content-Type': 'application/json;charset=UTF-8',
                'Authorization': `Bearer ${token.token}`
            },
        })
        .then(res => {
            if (!res.ok) {
                throw new Error('Network response was not ok');
            }
            return res.json();
        })
        .then((data) => {
            if (data && data.data && Array.isArray(data.data)) {
                setProblemList(data.data);
            } else {
                throw new Error('Invalid data format in the response');
            }
        })
        .catch(err => console.error('Error fetching problems:', err));
    }, []);

    return (
        <tbody className='text-white'>
            {problemList.map((doc, idx) => {
                const difficultyColor =
                    doc.difficulty === "Easy"
                        ? "text-dark-green-s"
                        : doc.difficulty === "Medium"
                        ? "text-dark-yellow"
                        : "text-dark-pink";
                return (
                    <tr className={`${idx % 2 === 1 ? 'bg-dark-layer-1' : ''}`} key={doc.id}>
                        <th className='px-2 py-4 font-medium whitespace-nowrap text-dark-green-s'>
                            <BsCheckCircle fontSize={"18"} width='18' />
                        </th>
                        <td className='px-6 py-4'>
                            {/* <Link href={`/problems/${doc.id}`}> */}
                                <a className="hover:text-blue-600 cursor-pointer">{doc.title}</a>
                            {/* </Link> */}
                        </td>
                        <td className={`px-6 py-4 ${difficultyColor}`}>
                            {doc.difficulty}
                        </td>
                        <td className='px-6 py-4'>{doc.tagId[0]}</td>
                        <td className='px-6 py-4'>Coming soon</td>
                    </tr>
                );
            })}
        </tbody>
    );
};

export default ProblemsTable;
