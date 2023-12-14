'''
Program Name: RavenModule_GenerateAlternatives
Author: Sergio A. Covarrubias
Change Log:
    2022-11-24 rev00:
        - Obtain the initial element of the alternatives, 
        calculating the last element of the progression that
        also corresponds to the correct answer.
        - The increment in every behavior, of the alternatives' progression, is changed to a random value.
    2022-11-25 rev01:
        Generate alternatives following a new progression defined by the 
        initial element
    2022-11-27 rev02:
        Try to correct the copy of the dictionary elements, 
        because the same node is repeated.
    2022-11-29 rev03:
        The copy of the dictionary and its elements is fixed.
    2022-11-30 rev04:
        Round values to put in the dictionary.
        Test the generation in Unity.
    2023-02-15 rev05:
        TODO: seguir el orden de los itemType definidos en 'challengeFormat.json'.
                i.e.: el orden entre los sólidos y los outline 
'''


import json
import random
import copy
  
f = open('challengeFormat.json')
data = json.load(f)
f.close()

elementsQtty = data['elementsQty']

# INITIAL VALUE FOR THE ALTERNATIVES
item_count = 0
element_count = 0
initialElementFormat = None
for element in data['ElementFormatsList']:
    for item in element['ItemFormatsList']:
        for behavior in item['Behaviors']:
            behavior['initialValue'] = round(behavior['initialValue'] + behavior['increment'] * (elementsQtty -1), 2)
            if (behavior['type'] == 'Sides'):
                randomIncrement = round(random.randint(-8, 8) / 4.0, 0)
            else:
                # randomIncrement = round(random.randint(-300, 300) / 600, 1)
                randomIncrement = round(random.randint(-6, 6) * 0.125, 2)
            behavior['increment'] = randomIncrement

with open("AnswerChallengeFormat.json", "w") as f:
    json.dump(data, f, indent=4)

# INCLUDE RIGHT ANSWER AND 4 MORE ALTERNATIVES
result_data = copy.deepcopy(data)
result_data['elementsQty'] = 5
tempElement = result_data['ElementFormatsList'][0]
result_data['ElementFormatsList'] = []
result_data['ElementFormatsList'].append(copy.deepcopy(data['ElementFormatsList'][0]))
for n in range(result_data['elementsQty'] -1):
    for tmp_item in tempElement['ItemFormatsList']:
        print('tmp_item[''itemType'']: ', tmp_item['itemType'])
        for tmp_behavior in tmp_item['Behaviors']:
            initVal = tmp_behavior['initialValue']
            str_inc  = tmp_behavior['increment']
            if (behavior['type'] == 'Sides' and (initVal + str_inc) < 3):
                tmp_behavior['initialValue'] = 3
            else:
                if (initVal>3 or initVal<0.5):
                    str_inc = str_inc*-1.0
                tmp_behavior['initialValue'] = round(initVal + str_inc, 2)
            
            print(f"{tmp_behavior['type']} -> tmp_behavior['initialValue'] = {initVal} + {str_inc} = {tmp_behavior['initialValue']}")
    print()
    result_data['ElementFormatsList'].append(copy.deepcopy(tempElement))

data_json = json.dumps(result_data, indent = 4)

with open("AlternativesChallengeFormat.json", "w") as outfile:
    outfile.write(data_json)

outfile.close()

f = open('AlternativesChallengeFormat.json')
data_json = json.load(f)
f.close()

dictionary = {"Challenge": data, "Alternatives": data_json}

# Dump all the Python objects into a single JSON file.
with open("PlayerChallenge.json", "w") as f:
    json.dump(dictionary, f, indent=4)