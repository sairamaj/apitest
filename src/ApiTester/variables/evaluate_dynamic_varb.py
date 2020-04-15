from variables.datetime_variable import DateVariable
from variables.random_variable import RandomVariable
from variables.guid_variable import GuidVariable


def evaluate_dynamic(variable):
    evaluator = { 
        "$today_date": DateVariable(variable),
        "$random" : RandomVariable(variable),
        "$guid" : GuidVariable(variable)
        }

    parts = variable.split(' ')
    func = evaluator.get(parts[0],None)
    if func == None:
        raise ValueError(f"{parts[0]} is not supported.")
    return func.eval()

if __name__ == "__main__":
    val = evaluate_dynamic('$today_date format=%Y-%m-%d days=10')
    print(val)