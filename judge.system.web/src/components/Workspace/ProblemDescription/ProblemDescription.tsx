import { useEffect, useState } from "react";
import {
  AiFillLike,
  AiFillDislike,
  AiOutlineLoading3Quarters,
  AiFillStar,
} from "react-icons/ai";
import { BsCheck2Circle } from "react-icons/bs";
import { TiStarOutline } from "react-icons/ti";
import { toast } from "react-toastify";

type ProblemDescriptionProps = {
  problemId: string;
  _solved: boolean;
};

type TestCase = {
  item1: Record<string, any>;
  item2: any;
};

export type ProblemDetail = {
  title: string;
  description: string;
  testCases: TestCase[];
  functionName: string;
  returnType: string;
  };

const ProblemDescription: React.FC<ProblemDescriptionProps> = ({
  problemId,
  _solved,
}) => {
  const [problem, setProblem] = useState<ProblemDetail | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [liked, setLiked] = useState(false);
  const [disliked, setDisliked] = useState(false);
  const [starred, setStarred] = useState(false);
  const [updating, setUpdating] = useState(false);

  useEffect(() => {
    const fetchProblemDetails = async () => {
      try {
        setLoading(true);
        const response = await fetch(
          `http://localhost:5107/api/Problems/GetProblemDetail?problemId=${problemId}`,
          {
            method: "GET",
            mode: "cors",
            headers: {
              Accept: "application/json, text/plain",
              "Content-Type": "application/json;charset=UTF-8",
            },
          }
        );
        const data = await response.json();
        setProblem(data.data);
      } catch (error) {
        toast.error("Failed to fetch problem details");
      } finally {
        setLoading(false);
      }
    };

    fetchProblemDetails();
  }, [problemId]);

  const handleLike = async () => {
    // Handle like functionality here
  };

  const handleDislike = async () => {
    // Handle dislike functionality here
  };

  const handleStar = async () => {
    // Handle star functionality here
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  return (
    <div className="bg-dark-layer-1">
      {/* TAB */}
      <div className="flex h-11 w-full items-center pt-2 bg-dark-layer-2 text-white overflow-x-hidden">
        <div
          className={
            "bg-dark-layer-1 rounded-t-[5px] px-5 py-[10px] text-xs cursor-pointer"
          }
        >
          Description
        </div>
      </div>

      <div className="flex px-0 py-4 h-[calc(100vh-94px)] overflow-y-auto">
        <div className="px-5">
          {/* Problem heading */}
          <div className="w-full">
            <div className="flex space-x-4">
              <div className="flex-1 mr-2 text-lg text-white font-medium">
                {problem?.title}
              </div>
            </div>
            <div className="flex items-center mt-3">
              {(liked || _solved) && (
                <div className="rounded p-[3px] ml-4 text-lg transition-colors duration-200 text-green-s text-dark-green-s">
                  <BsCheck2Circle />
                </div>
              )}
              <div
                className="flex items-center cursor-pointer hover:bg-dark-fill-3 space-x-1 rounded p-[3px] ml-4 text-lg transition-colors duration-200 text-dark-gray-6"
                onClick={handleLike}
              >
                {liked && !updating && (
                  <AiFillLike className="text-dark-blue-s" />
                )}
                {!liked && !updating && <AiFillLike />}
                {updating && (
                  <AiOutlineLoading3Quarters className="animate-spin" />
                )}

                <span className="text-xs">{/* {currentProblem.likes} */}</span>
              </div>
              <div
                className="flex items-center cursor-pointer hover:bg-dark-fill-3 space-x-1 rounded p-[3px] ml-4 text-lg transition-colors duration-200 text-green-s text-dark-gray-6"
                onClick={handleDislike}
              >
                {disliked && !updating && (
                  <AiFillDislike className="text-dark-blue-s" />
                )}
                {!disliked && !updating && <AiFillDislike />}
                {updating && (
                  <AiOutlineLoading3Quarters className="animate-spin" />
                )}

                <span className="text-xs">
                  {/* {currentProblem.dislikes} */}
                </span>
              </div>
              <div
                className="cursor-pointer hover:bg-dark-fill-3 rounded p-[3px] ml-4 text-xl transition-colors duration-200 text-green-s text-dark-gray-6"
                onClick={handleStar}
              >
                {starred && !updating && (
                  <AiFillStar className="text-dark-yellow" />
                )}
                {!starred && !updating && <TiStarOutline />}
                {updating && (
                  <AiOutlineLoading3Quarters className="animate-spin" />
                )}
              </div>
            </div>

            {/* Problem Statement(paragraphs) */}
            <div className="text-white text-sm mt-4">
              <p>{problem?.description}</p>
            </div>

            {/* Examples */}
            <div className="mt-4">
              {problem?.testCases.map((example, index) => (
                <div key={index}>
                  <p className="font-medium text-white">Example {index + 1}:</p>
                  <div className="example-card">
                    <pre>
                      <strong className="text-white">Input: </strong>
                      {Object.entries(example.item1).map(([key, value]) => (
                        <span key={key}>{`${key}: ${JSON.stringify(
                          value
                        )}, `}</span>
                      ))}
                      <br />
                      <strong>Output:</strong> {JSON.stringify(example.item2)}
                    </pre>
                  </div>
                </div>
              ))}
            </div>

            {/* Constraints (if any) */}
            <div className="my-8 pb-4">
              <div className="text-white text-sm font-medium">Constraints:</div>
              <ul className="text-white ml-5 list-disc">
                {/* Add constraints here if needed */}
              </ul>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ProblemDescription;
